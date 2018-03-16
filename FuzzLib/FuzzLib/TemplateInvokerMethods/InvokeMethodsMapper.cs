using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FuzzLib.TemplateInvokerMethods
{
    public class InvokeMethodsMapper : IInvokeMethodsMapper
    {
        private readonly List<InvokerMethodWrapperType> _cache;
        public InvokeMethodsMapper()
        {
            _cache = new List<InvokerMethodWrapperType>();

            var type = typeof(ITemplateInvokerMethod);
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(GetLoadableTypes).Where(t => t.GetInterfaces().Contains(type)).ToList();

            foreach (var invokerMethodType in types)
            {
                var metadate = invokerMethodType
                    .GetMethods()
                    .Select(method => new
                    {
                        InvokeMethod = method,
                        InvokeAttribute = method.GetCustomAttributes<InvokerMethodAttribute>().FirstOrDefault()
                    })
                    .FirstOrDefault(attr => attr.InvokeAttribute != null && !string.IsNullOrWhiteSpace(attr.InvokeAttribute.Name));

                if (metadate != null)
                    _cache.Add(new InvokerMethodWrapperType(metadate.InvokeAttribute.Name, invokerMethodType, metadate.InvokeMethod));
            }
        }

        public IEnumerable<InvokerMethodWrapper> GetInvokers()
        {
            return _cache.Select(item => new InvokerMethodWrapper(
                item.Name,
                (ITemplateInvokerMethod)Activator.CreateInstance(item.InvokerMethodType),
                item.InvokerMethodInfo)).ToList();
        }

        public IEnumerable<string> GetMethods()
        {
            return _cache.Select(item => item.Name).ToList();
        }

        private IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}
