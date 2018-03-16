using System.Collections.Generic;
using System.Linq;

namespace FuzzLib.Functions
{
    public class FunctionsContainer : IFunctionsContainer
    {
        private readonly IList<IFunction> _functions;

        public FunctionsContainer()
        {
            _functions = new List<IFunction>();
        }

        public void Add(IFunction function)
        {
            if (!_functions.Select(item => item.Format()).Contains(function.Format()))
                _functions.Add(function);
        }

        public IList<IFunction> GetFunctions()
        {
            return _functions;
        }

        public void Clear()
        {
            _functions.Clear();
        }

        public IFunctionsContainer Clone()
        {
            var functionsContainer = new FunctionsContainer();

            foreach (var fn in _functions)
                functionsContainer.Add(fn);

            return functionsContainer;
        }
    }
}
