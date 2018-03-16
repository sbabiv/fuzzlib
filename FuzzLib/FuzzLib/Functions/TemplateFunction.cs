using System.Collections.Generic;
using System.Linq;

namespace FuzzLib.Functions
{
    public class TemplateFunction : IFunction
    {
        private readonly IList<Parameter> _parameters;

        public string Name { get; set; }

        public TemplateFunction(string name)
        {
            _parameters = new List<Parameter>();
            Name = name;
        }

        public void Add(Parameter argument)
        {
            argument.Index = _parameters.Count;
            _parameters.Add(argument);
        }

        public IList<Parameter> GetParameters()
        {
            return _parameters;
        }

        public string Format()
        {
            return $"{Name}({string.Join(",", _parameters.Select(argument => argument))})";
        }
    }
}
