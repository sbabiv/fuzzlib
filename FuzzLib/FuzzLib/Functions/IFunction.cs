using System.Collections.Generic;

namespace FuzzLib.Functions
{
    public interface IFunction
    {
        /// <summary>
        /// function name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// function format
        /// </summary>
        /// <returns></returns>
        string Format();

        /// <summary>
        /// add function parameter
        /// </summary>
        /// <param name="argument"></param>
        void Add(Parameter argument);

        /// <summary>
        /// get function parameters
        /// </summary>
        /// <returns></returns>
        IList<Parameter> GetParameters();
    }
}
