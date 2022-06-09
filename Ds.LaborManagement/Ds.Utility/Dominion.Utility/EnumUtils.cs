using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Dominion.Utility
{
    public class EnumUtils
    {
        public static string GetEnumDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            if (fieldInfo == null)
            {
                return null;
            }

            var attributes =
                (DescriptionAttribute[]) fieldInfo.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static List<string> GetEnumDescriptions<T>()
        {
            var result = new List<string>();
            foreach (T val in Enum.GetValues(typeof(T)))
            {
                result.Add(GetEnumDescription(val as Enum));
            }

            return result;
        }

        public static Dictionary<int, string> GetDictionaryFromEnum<T>()
        {
            var result = new Dictionary<int, string>();
            foreach (T enumValue in Enum.GetValues(typeof(T)))
            {
                result.Add(Convert.ToInt32(enumValue), GetEnumDescription(enumValue as Enum));
            }

            return result;
        }
    }
}