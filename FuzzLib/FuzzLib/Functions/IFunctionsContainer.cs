using System.Collections.Generic;

namespace FuzzLib.Functions
{
    public interface IFunctionsContainer
    {
        void Add(IFunction function);
        IList<IFunction> GetFunctions();
        void Clear();
        IFunctionsContainer Clone();
    }
}
