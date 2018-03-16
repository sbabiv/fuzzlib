using System.Collections.Generic;

namespace FuzzLib.Parser
{
    public interface ITemplateParser
    {
        void SetMethods(IList<string> methods);
        TemplateContext Parse(string content, bool optimizationHtmlCode);
    }
}
