using System.Collections.Generic;

using FuzzLib.Functions;

namespace FuzzLib.Parser
{
    public interface ITemplateContext
    {
        string Content { get; }
        IFunctionsContainer FunctionsContainer { get; }
        IList<Parameter> Parameters { get; }
    }
}
