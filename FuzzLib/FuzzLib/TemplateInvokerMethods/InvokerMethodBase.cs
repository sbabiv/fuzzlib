using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

using FuzzLib.Functions;

namespace FuzzLib.TemplateInvokerMethods
{
    public abstract class InvokerMethodBase<T>
    {
        protected ConcurrentDictionary<string, T> _cache;

        protected InvokerMethodBase()
        {
            _cache = new ConcurrentDictionary<string, T>();
        }

        protected string GetValueByParam(XElement node, Parameter parameter)
        {
            return (string.IsNullOrEmpty(parameter.Loop)
                ? node.Element(parameter.ToString())
                : node.XPathSelectElement(parameter.ToString())).Value;
        }

        protected List<string> GetValueByParamLoop(XElement node, Parameter parameter)
        {
            if (!string.IsNullOrEmpty(parameter.Loop))
            {
                return node.Descendants(parameter.Loop)
                     .Descendants("param")
                     .Elements(parameter.Name)
                     .Select(t => t.Value)
                     .ToList();
            }

            return new List<string> { node.XPathSelectElement(parameter.ToString()).Value };
        }
    }
}
