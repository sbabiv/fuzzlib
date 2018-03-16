using System;
using System.Collections.Generic;

using FuzzLib.TemplateInvokerMethods;

namespace FuzzLib.Binary
{
    public interface IBinaryTemplate
    {
        string Render(Dictionary<string, object> data);
        void SetHandlers(Func<IEnumerable<InvokerMethodWrapper>> methods);
    }
}
