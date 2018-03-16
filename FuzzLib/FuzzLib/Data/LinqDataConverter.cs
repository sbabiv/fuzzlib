using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FuzzLib.Data
{
    public class LinqDataConverter : IDataConverter
    {
        public Dictionary<string, object> ToData(XElement data)
        {
            var result = new Dictionary<string, object>();
            if (data != null)
            {
                foreach (var el in data.Elements())
                {
                    var descendants = el.Elements().ToArray();
                    if (descendants.Any())
                    {
                        var descendantsItems = descendants.Select(
                            descendant => descendant.Elements().ToDictionary(
                                item => string.Format("{0}.{1}", descendant.Parent.Name, item.Name.ToString()),
                                item => (object)item.Value)).ToList();

                        if (descendantsItems.Any())
                            result.Add(el.Name.ToString(), descendantsItems);
                        continue;
                    }
                    result.Add(el.Name.ToString(), el.Value);
                }
            }

            return result;
        }
    }
}
