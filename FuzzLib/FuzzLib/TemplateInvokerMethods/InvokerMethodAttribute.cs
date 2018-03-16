using System;

namespace FuzzLib.TemplateInvokerMethods
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InvokerMethodAttribute : Attribute
    {
        public InvokerMethodAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
