using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;

using FuzzLib.Parser;
using Microsoft.CSharp;

namespace FuzzLib.Binary
{
    public class BinaryTemplatesFactory
    {
        private readonly ITemplateParser _templateParser;

        public BinaryTemplatesFactory(ITemplateParser templateParser)
        {
            _templateParser = templateParser;
        }

        public BinaryTemplate Compile(string template, bool optimizationHtmlCode = false)
        {
            var compiler = new CSharpCodeProvider();
            var parms = new CompilerParameters { GenerateExecutable = false, GenerateInMemory = true };

            parms.ReferencedAssemblies.Add("System.dll");
            parms.ReferencedAssemblies.Add("System.Core.dll");

            var templateContent = _templateParser.Parse(template, optimizationHtmlCode);
            var cs = CSharp().Replace("{0}", templateContent.Content);
            var result = compiler.CompileAssemblyFromSource(parms, cs);
            var builderType = result.CompiledAssembly.GetType("BinaryTemplates.BinaryBuilder");
            var instance = Activator.CreateInstance(builderType);
            var addHanlder = (Action<object, MethodInfo, string>)Delegate.CreateDelegate(typeof(Action<object, MethodInfo, string>), instance, "AddHandler");
            var clearHandler = (Action)Delegate.CreateDelegate(typeof(Action), instance, "ClearHandlers");
            var renderHandler = (Func<Dictionary<string, object>, string>)Delegate.CreateDelegate(typeof(Func<Dictionary<string, object>, string>), instance, "Render");

            return new BinaryTemplate(new BinaryBuilderMembers(addHanlder, clearHandler, renderHandler, templateContent.FunctionsContainer));
        }

        private static string CSharp()
        {
            return @"
            using System;
            using System.Linq;
            using System.Collections.Generic;
            using System.Reflection;            

            namespace BinaryTemplates
            {             
                public class BinaryBuilder 
                {
                    private delegate string Handler(params string[] list);
                    private readonly Dictionary<string, Handler> handlers;
                    
                    public BinaryBuilder()
                    {
                        handlers = new Dictionary<string, Handler>();
                    }

                    public void AddHandler(object instanse, MethodInfo info, string name)
                    {
                        handlers.Add(name, (Handler) info.CreateDelegate(typeof (Handler), instanse));
                    }

                    public void ClearHandlers()
                    {
                        handlers.Clear();
                    }

                    public string Render(Dictionary<string, object> data)
                    {
                        " + "return String.Concat(\"{0}\");" + @"
                    }

                    private IEnumerable<Dictionary<string, object>> GetList(Dictionary<string, object> data, string key)
                    {
                        try
                        {
                            var list = data[key] as List<Dictionary<string, object>>;
                            return (list ?? new List<Dictionary<string, object>>());
                        }
                        catch (KeyNotFoundException)
                        {
                            throw new KeyNotFoundException(""Value on the key '"" + key + ""' is not found"");
                        }
                    }

                    private string GetVal(Dictionary<string, object> data, string key)
                    {
                        try
                        {
                            return data[key].ToString();
                        }
                        catch (KeyNotFoundException)
                        {
                            throw new KeyNotFoundException(""Value on the key '"" + key + ""' is not found"");
                        }
                    }
                }
            }";
        }
    }
}
