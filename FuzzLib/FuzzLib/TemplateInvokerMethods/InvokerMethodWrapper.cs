using System.Reflection;

namespace FuzzLib.TemplateInvokerMethods
{
    public class InvokerMethodWrapper
    {
        public string Name { get; private set; }
        public ITemplateInvokerMethod Instance { get; private set; }
        public MethodInfo InvokerMethodInfo { get; private set; }

        public InvokerMethodWrapper(string name, ITemplateInvokerMethod instance, MethodInfo invokerMethodInfo)
        {
            Name = name;
            Instance = instance;
            InvokerMethodInfo = invokerMethodInfo;
        }
    }
}
