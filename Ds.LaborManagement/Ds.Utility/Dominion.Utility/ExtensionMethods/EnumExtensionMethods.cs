using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.ExtensionMethods
{
    public static class EnumExtensionMethods
    {
        /// <summary>
        /// Returns the enum's integer equivalent as a string.
        /// </summary>
        /// <param name="eVal">Enum value to convert to string.</param>
        /// <returns></returns>
        /// <example>
        /// public enum Rank 
        /// {
        ///     Low    = 1,
        ///     Medium = 2,
        ///     High   = 3
        /// }
        /// 
        /// public string DisplayRankMessage()
        /// {
        ///     Console.Write("You have " + Rank.High.ToValueString() + " stars!!");
        /// }
        /// 
        /// Outputs:
        /// "You have 3 stars!!"
        /// </example>
        public static string ToValueString(this Enum eVal)
        {
            return eVal.ToString("D");
        }

        /// <summary>
        /// Converts the specified text of the name or numeric value of one or more enumerated constants to an 
        /// equivalent enumerated constant.
        /// </summary>
        /// <typeparam name="TEnum">Type of enumeration to convert to.</typeparam>
        /// <param name="enumVal">String representation of the enumeration to convert. Can be numeric or name 
        /// equivalent.</param>
        /// <param name="ignoreCase">true to ignore case; false to regard case</param>
        /// <returns></returns>
        public static TEnum ToEnum<TEnum>(this string enumVal, bool ignoreCase = false)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), enumVal, ignoreCase);
        }

        /// <summary>
        /// Returns a list of all enum types currently defined in <see cref="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TEnum> ToEnumerable<TEnum>()
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }
    }
}
