using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Dominion.Utility.Enums
{
    public static class EnumHelper
    {
        /// <summary>
        /// Returns <see cref="DescriptionAttribute"/> for enum value if it is specified for that value,
        /// else returns the name of that enum value.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription<TEnum>(this TEnum value) where TEnum : Enum
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        /// <summary>
        /// Gets enumerable of all enum values for a given <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TEnum> GetEnumEnumerable<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>();
        }

        /// <summary>
        /// Gets ordred enumerable of all enum values for a given <typeparamref name="TEnum"/>, ordered ascendingly according to enum value.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static IOrderedEnumerable<TEnum> GetEnumOrderedEnumerable<TEnum>() where TEnum : Enum
        {
            return GetEnumEnumerable<TEnum>()
                .OrderBy(x => x);
        }

        /// <summary>
        /// Maps specificed enum values to dictionary of (enum value => enum description).
        /// </summary>
        /// <typeparam name="TEnum">This is most useful when all values for <typeparamref name="TEnum"/> have a <see cref="DescriptionAttribute"/>.</typeparam>
        /// <param name="enums">Collection to map to Enum Description.</param>
        /// <returns>Dictionary of (enum value => enum description) key, value pairs.</returns>
        public static IDictionary<TEnum, string> GetEnumDescriptionDictionary<TEnum>(IEnumerable<TEnum> enums) where TEnum : Enum
        {
            return enums.ToDictionary(x => x, x => x.GetEnumDescription());
        }

        /// <summary>
        /// Maps all enum values of the specified <typeparamref name="TEnum"/> to dictionary of (enum value => enum description).
        /// </summary>
        /// <typeparam name="TEnum">This is most useful when all values for <typeparamref name="TEnum"/> have a <see cref="DescriptionAttribute"/>.</typeparam>
        /// <returns>Dictionary of (enum value => enum description) key, value pairs.</returns>
        public static IDictionary<TEnum, string> GetEnumDescriptionDictionary<TEnum>() where TEnum : Enum
        {
            return GetEnumDescriptionDictionary(GetEnumEnumerable<TEnum>());
        }

        /// <summary>
        /// Maps all enum values of the specified <typeparamref name="TEnum"/> to an ordered list of enum descriptions.
        /// </summary>
        /// <typeparam name="TEnum">This is most useful when all values for <typeparamref name="TEnum"/> have a <see cref="DescriptionAttribute"/>.</typeparam>
        /// <returns>Ordered list of enum descriptions, ordered by the source enum values</returns>
        public static IList<string> GetEnumDescriptionsOrderedByEnumValue<TEnum>() where TEnum : Enum
        {
            return GetEnumDescriptionDictionary<TEnum>()
                .OrderBy(x => x.Key)
                .Select(x => x.Value)
                .ToList();
        }
    }
}
