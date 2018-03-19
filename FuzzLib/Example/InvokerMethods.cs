using System;
using FuzzLib.TemplateInvokerMethods;

namespace Example
{
    public class HelloWorldMethod : InvokerMethodBase<string>, ITemplateInvokerMethod
    {
        [InvokerMethod("HelloWorld")]
        public string InvokeMethod(params string[] args)
        {
            return $"hello world {Guid.NewGuid()}";
        }
    }

    public class RemoveLink : InvokerMethodBase<string>, ITemplateInvokerMethod
    {
        private const string URL = "http://www.car-shop.{0}/unsubscribe/{1}/{2}";

        [InvokerMethod("RemoveLink")]
        public string InvokeMethod(params string[] args)
        {
            var key = String.Join(":", args);
            if (_cache.ContainsKey(key))
                return _cache[key];

            var result = string.Format(URL, args[0], args[1], args[2]);
            _cache.TryAdd(key, result);

            return result;
        }
    }
}
