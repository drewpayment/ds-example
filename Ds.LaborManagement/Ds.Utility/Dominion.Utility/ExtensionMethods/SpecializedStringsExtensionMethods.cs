using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.ExtensionMethods
{
    public static class SpecializedStringsExtensionMethods
    {
        public static string ToCsArrStr(
            this IEnumerable<string> list,
            bool addNewline = true,
            bool spaceAfterComma = false,
            bool trimEndSeparator = false,
            int leadingTabs = 0)
        {
            var separator = spaceAfterComma ? ", " : ",";
            var str = list.ToSpecialString("\"", separator, addNewline, trimEndSeparator, "\t", leadingTabs);
            return str;
        }

        public static string ToSpecialString(this IEnumerable<string> list, string surroundingStr, string separator, bool addNewline, bool trimEndSeparator, string prefixStr = null, int prefixCount = 0)
        {
            var sb = new StringBuilder();
            foreach (var str in list)
                str.ToSpecialString(sb, surroundingStr, separator, addNewline, false, prefixStr, prefixCount);

            if (trimEndSeparator)
                return sb.ToString().TrimEnd(separator.ToCharArray());

            return sb.ToString();
        }

        public static string ToSpecialString(this string str, StringBuilder existingSb, string surroundingStr, string separator, bool addNewline, bool trimEndSeparator, string prefixStr = null, int prefixCount = 0)
        {
            var sb = existingSb ?? new StringBuilder();

            for (int i = 0; i < prefixCount; i++)
                sb.Append(prefixStr ?? string.Empty);

            sb.Append(surroundingStr).Append(str).Append(surroundingStr).Append(separator);

            if (addNewline)
                sb.AppendLine();

            if (trimEndSeparator)
                return sb.ToString().TrimEnd(separator.ToCharArray());

            return sb.ToString();
        }

        //public static string ToCsArrStr(this string str)
        //{
        //    return $@"""{str}"",";
        //}
    }
}
