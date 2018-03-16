using System.Collections.Generic;

namespace FuzzLib.TemplateInvokerMethods
{
    public interface IInvokeMethodsMapper
    {
        IEnumerable<InvokerMethodWrapper> GetInvokers();
        IEnumerable<string> GetMethods();
    }
}
