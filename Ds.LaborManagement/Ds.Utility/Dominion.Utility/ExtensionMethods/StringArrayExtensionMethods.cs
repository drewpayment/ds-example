using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.ExtensionMethods
{
    public static class StringArrayExtensionMethods
    {
        
        public static string ToJoinedString(this string[] arr,string separator, int startIndex,int? endIndex = null)
        {
            string[] strArray = arr;
            endIndex = endIndex ?? arr.Length;

            strArray = arr.Skip(startIndex).Take((endIndex ?? 0) - startIndex).ToArray();
            return string.Join(separator, strArray);

        }
        public static string ToJoinedString(this string[] arr, string separator =",")
        {
            return String.Join(separator, arr);
        }
    }
}
