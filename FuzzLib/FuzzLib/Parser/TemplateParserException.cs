using System;

namespace FuzzLib.Parser
{
    public class TemplateParserException : Exception
    {
        public TemplateParserException()
        {
        }

        public TemplateParserException(string message) : base(message)
        {
        }

        public TemplateParserException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
