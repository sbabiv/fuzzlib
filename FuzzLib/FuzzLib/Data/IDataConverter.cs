using System.Collections.Generic;
using System.Xml.Linq;

namespace FuzzLib.Data
{
    public interface IDataConverter
    {
        Dictionary<string, object> ToData(XElement data);
    }
}
