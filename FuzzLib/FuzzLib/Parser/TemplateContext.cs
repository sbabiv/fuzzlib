using System.Collections.Generic;

using FuzzLib.Functions;

namespace FuzzLib.Parser
{
    public class TemplateContext : ITemplateContext
    {
        public string Content { get; }
        public IFunctionsContainer FunctionsContainer { get; }
        public IList<Parameter> Parameters { get; }

        public TemplateContext(string content, IFunctionsContainer functionsContainer, IEnumerable<Parameter> parameters)
        {
            Parameters = new List<Parameter>();

            Content = content;
            FunctionsContainer = functionsContainer.Clone();

            foreach (var parameter in parameters)
                Parameters.Add(parameter);
        }
    }
}
