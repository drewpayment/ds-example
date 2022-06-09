using System;
//using System.ComponentModel;

namespace Dominion.Core.Dto.Employee
{
    /// <summary>
    /// TODO: This should be consolidated down to one enum. Right now a similar one resides in Onboarding.
    /// </summary>
    /// <remarks>
    /// Changes to the names of these values will affect what strings parse _to_ the value which name was changed.
    /// For example, changing the name of <see cref="MaritalStatus.Single=2"/> to <c>MaritalStatus.Unmarried=2</c>
    /// would cause <see cref="MaritalStatusEnumExtensions.ParseStringToMaritalStatusEnum(string)"/> to parse
    /// "Unmarried" => <c>MaritalStatus.Unmarried</c>, instead of "Single" => <see cref="MaritalStatus.Single"/>.
    /// </remarks>
    public enum MaritalStatus : byte
    {
        //[Description("Unspecified")]
        Unspecified = 0,
        //[Description("Married")]
        Married = 1,
        //[Description("Single")]
        Single = 2
    }

    public static class MaritalStatusEnumExtensions
    {
        /// <summary>
        /// Maps some extra input string values to <see cref="Enum.ToString"/> on matching <see cref="MaritalStatus"/> .
        /// </summary>
        /// <param name="value"></param>
        /// <returns>
        /// Output string is never null, instead returns empty string if invalid input.
        /// Output is suitable for passing into <see cref="ParseStringToMaritalStatusEnum"/>.
        /// </returns>
        public static string NormalizeInputStringToDescriptions(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if (value.Length == 1)
            {
                switch (value.ToUpperInvariant())
                {
                    case "M":
                        return MaritalStatus.Married.ToString();
                    case "S":
                        return MaritalStatus.Single.ToString();
                    case "N":
                    case "U":
                        return MaritalStatus.Unspecified.ToString();
                    default:
                        return string.Empty;
                }
            }
            else if (value.ToUpperInvariant() == "NOTSPECIFIED" || value.ToUpperInvariant() == "NOT SPECIFIED")
            {
                return MaritalStatus.Unspecified.ToString();
            }
            else
            {
                return value;
            }
        }

        public static MaritalStatus? ParseStringToMaritalStatusEnum(string value)
        {
            MaritalStatus result;

            string _value = NormalizeInputStringToDescriptions(value);

            if (Enum.TryParse(_value, ignoreCase: true, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static MaritalStatus CoerceToNonNullable(MaritalStatus? value)
        {
            return value ?? MaritalStatus.Unspecified;
        }
    }
}
