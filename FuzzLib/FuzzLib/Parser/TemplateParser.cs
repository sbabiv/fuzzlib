using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using FuzzLib.Functions;

namespace FuzzLib.Parser
{
    public class TemplateParser : ITemplateParser
    {
        private readonly IFunctionsContainer _functionsContainer;
        private readonly IList<Parameter> _parameters;
        private IList<string> _methods;

        public TemplateParser(IFunctionsContainer functionsContainer)
        {
            _functionsContainer = functionsContainer;
            _parameters = new List<Parameter>();
        }

        public void SetMethods(IList<string> methods)
        {
            _methods = methods;
        }

        public TemplateContext Parse(string content, bool optimizationHtmlCode)
        {
            content = Encode(content, optimizationHtmlCode);
            content = content.Replace("{%endfor%}", "\"))) , \"");

            foreach (var expression in Regex.Matches(content, @"{%for:\w+%}"))
            {
                var key = expression.ToString().Replace("{%for:", "").Replace("%}", "");
                content = content.Replace(expression.ToString(), string.Format("\", string.Concat((GetList(data,\"{0}\")).Select(item => string.Concat(\"", key));
            }

            content = ParseFunction(content);
            foreach (var variable in Regex.Matches(content, @"({%\w+%}|{%=\w+.\w+%})"))
            {
                var varFormatting = variable.ToString().Replace("{%=", "").Replace("{%", "").Replace("%}", "");
                var parameter = new Parameter(varFormatting);

                if (!_parameters.Contains(parameter)) _parameters.Add(parameter);

                varFormatting = variable.ToString().StartsWith("{%=")
                    ? string.Format("\",GetVal(item,\"{0}\"),\"", varFormatting)
                    : string.Format("\",GetVal(data,\"{0}\"),\"", varFormatting);
                content = content.Replace(variable.ToString(), varFormatting);
            }
            var context = new TemplateContext(content, _functionsContainer, _parameters);

            _parameters.Clear();
            _functionsContainer.Clear();

            return context;
        }

        private string ParseFunction(string content)
        {
            if (_methods == null) return content;
            RiseNoEmplementationException(content);
            foreach (var functionName in _methods)
            {
                var pattern = @"({%" + functionName + @"\(\)%})|({%" + functionName + @"\(({%\w+%}|{%=\w+.\w+%}|\\\""\w+\\\""){1}(,({%\w+%}|{%=\w+.\w+%}|\\\""\w+\\\""))*\)%})";
                foreach (var expression in Regex.Matches(content, pattern))
                {
                    var templateFunction = new TemplateFunction(functionName);
                    var function = expression.ToString();
                    foreach (var variable in Regex.Matches(function, @"({%\w+%}|{%=\w+.\w+%})"))
                    {
                        var varFormatting = variable.ToString()
                            .Replace("{%=", "")
                            .Replace("{%", "")
                            .Replace("%}", "");
                        templateFunction.Add(new Parameter(varFormatting));
                        varFormatting = variable.ToString().StartsWith("{%=")
                            ? string.Format("GetVal(item,\"{0}\")", varFormatting)
                            : string.Format("GetVal(data,\"{0}\")", varFormatting);
                        function = function.Replace(variable.ToString(), varFormatting);
                    }
                    /*replace Handler[funcname](args)*/
                    content = content.Replace(expression.ToString(), function
                        .Replace(@"\""", "\"")
                        .Replace("{%", "\",")
                        .Replace("%}", ",\"")
                        .Replace(functionName, string.Format("handlers[\"{0}\"]", functionName)));
                    _functionsContainer.Add(templateFunction);
                }
            }

            return content;
        }

        private void RiseNoEmplementationException(string content)
        {
            var result = Regex.Matches(content, @"({%\w+\()").Cast<Match>().Select(item => item.Value.Replace("{%", "").Replace("(", "")).ToList();
            if (result.Any())
            {
                foreach (var function in result)
                {
                    if (!_methods.Contains(function))
                        throw new TemplateParserException(string.Format("Function {0} no implementation", function));
                }
            }
        }

        private string Encode(string content, bool optimizationHtmlCode)
        {
            /*исправить простой текст переносы строк в циклах*/
            content = optimizationHtmlCode
                ? string.Concat(content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).Select(item => item.Trim()))
                : content.Replace("\r", "\\r").Replace("\n", "\\n");

            return content.Replace("\"", @"\""");
        }
    }
}
