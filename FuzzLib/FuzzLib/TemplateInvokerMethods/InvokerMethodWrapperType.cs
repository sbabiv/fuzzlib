using System;
using System.Reflection;

namespace FuzzLib.TemplateInvokerMethods
{
    class InvokerMethodWrapperType
    {
        public string Name { get; private set; }
        public Type InvokerMethodType { get; private set; }
        public MethodInfo InvokerMethodInfo { get; private set; }

        public InvokerMethodWrapperType(string name, Type invokerMethodType, MethodInfo invokerMethodInfo)
        {
            Name = name;
            InvokerMethodType = invokerMethodType;
            InvokerMethodInfo = invokerMethodInfo;
        }
    }
}
