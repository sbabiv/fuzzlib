using System;
using System.Collections.Generic;
using System.Linq;

using FuzzLib.TemplateInvokerMethods;

namespace FuzzLib.Binary
{
    public class BinaryTemplate : IBinaryTemplate
    {
        private readonly BinaryBuilderMembers _members;
        private IEnumerable<InvokerMethodWrapper> _methods;
        private Func<IEnumerable<InvokerMethodWrapper>> _getMethods;

        public BinaryTemplate(BinaryBuilderMembers members)
        {
            _methods = new List<InvokerMethodWrapper>();
            _members = members;
        }

        public void SetHandlers(Func<IEnumerable<InvokerMethodWrapper>> getMethods)
        {
            _getMethods = getMethods;
            ResetHandlers();
        }

        public string Render(Dictionary<string, object> data)
        {
            try
            {
                return _members.RenderHanlder(data);
            }
            catch (KeyNotFoundException keyNotFoundException)
            {
                throw new BinaryTemplateException(keyNotFoundException.Message, keyNotFoundException);
            }
        }

        public void ResetHandlers()
        {
            _members.ClearHanlder();
            _methods = Filter(_getMethods());

            foreach (var method in _methods)
                _members.AddHandler(method.Instance, method.InvokerMethodInfo, method.Name);
        }

        public IEnumerable<InvokerMethodWrapper> Methods()
        {
            return _methods;
        }

        private IEnumerable<InvokerMethodWrapper> Filter(IEnumerable<InvokerMethodWrapper> methodWrappers)
        {
            var functions = _members.FunctionsContainer.GetFunctions().Select(item => item.Name).ToList();

            return methodWrappers.Where(methodWrapper => functions.Contains(methodWrapper.Name));
        }
    }
}
