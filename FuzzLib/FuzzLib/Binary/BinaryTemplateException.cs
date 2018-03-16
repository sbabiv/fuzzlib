using System;

namespace FuzzLib.Binary
{
    public class BinaryTemplateException : Exception
    {
        public BinaryTemplateException()
        {
        }

        public BinaryTemplateException(string message) : base(message)
        {
        }

        public BinaryTemplateException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
