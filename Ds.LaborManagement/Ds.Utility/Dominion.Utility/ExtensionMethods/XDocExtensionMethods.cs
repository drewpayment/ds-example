using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dominion.Utility.ExtensionMethods
{
    public static class XDocExtensionMethods
    {
        public static bool HasAttribute(this XElement elem, string attrName)
        {
            return !string.IsNullOrEmpty(elem.Attribute(attrName)?.Name?.LocalName);
        }
        public static string GetAttValue(this XElement elem, string attrName)
        {
            return elem.Attribute(attrName)?.Value;
        }

    }
}
