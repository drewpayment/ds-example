using Dominion.Utility.Validation.FluentValidate;
using FluentValidation;

namespace Dominion.Utility.Validation
{
    /// <summary>
    /// Creates extensions that can be used to build validators fluently.
    /// </summary>
    public static class ValidatorExtensions
    {
        /// <summary>
        /// Validates the given string against an SSN regex. Note: Validator will allow null and/or an empty string (use NotNull or NotEmpty to restrict further).
        /// </summary>
        /// <typeparam name="T">Type to validate.</typeparam>
        /// <param name="ruleBuilder">Rule Builder to add validation rule to.</param>
        /// <returns>Rule builder that can be used to further add validation rules fluently.</returns>
        public static IRuleBuilderOptions<T, string> SocialSecurityNumber<T>(this IRuleBuilder<T, string> ruleBuilder, 
            string errorMessage = null)
        {
            if (string.IsNullOrEmpty(errorMessage))
                return ruleBuilder.SetValidator(new SocialSecurityNumberValidator());

            return ruleBuilder.SetValidator(new SocialSecurityNumberValidator(errorMessage));
        }

        /// <summary>
        /// Validates the given string against Routing Number rules.
        /// Note: Validator will allow null and/or an empty string (use NotNull or NotEmpty to restrict further).
        /// </summary> 
        /// <remarks>Derived from https://github.com/DrShaffopolis/bank-routing-number-validator </remarks>
        /// <typeparam name="T">Type to validate.</typeparam>
        /// <param name="ruleBuilder">Rule Builder to add validation rule to.</param>
        /// <returns>Rule builder that can be used to further add validation rules fluently.</returns>
        public static IRuleBuilderOptions<T, string> RoutingNumber<T>(this IRuleBuilder<T, string> ruleBuilder,
            string errorMessage = null)
        {
            if (string.IsNullOrEmpty(errorMessage))
                return ruleBuilder.SetValidator(new RoutingNumberValidator());

            return ruleBuilder.SetValidator(new RoutingNumberValidator(errorMessage));
        }

        /// <summary>
        /// Validates the given property contains a valid enum value.
        /// </summary>
        /// <typeparam name="T">Type to validate.</typeparam>
        /// <typeparam name="TEnum">Type of enum to validate against.</typeparam>
        /// <param name="ruleBuilder">Rule Builder to add validation rule to.</param>
        /// <returns>Rule builder that can be used to further add validation rules fluently.</returns>
        public static IRuleBuilderOptions<T, TEnum> Enum<T, TEnum>(this IRuleBuilder<T, TEnum> ruleBuilder, 
            string errorMessage = null)
        {
            if (string.IsNullOrEmpty(errorMessage))
                return ruleBuilder.SetValidator(new EnumValidator());

            return ruleBuilder.SetValidator(new EnumValidator(errorMessage));
        }

        /// <summary>
        /// Validates the given property is convertable to an integer value.
        /// </summary>
        /// <typeparam name="T">Type to validate.</typeparam>
        /// <typeparam name="TElement">Type of property to validate.</typeparam>
        /// <param name="options">Rule builder.</param>
        /// <param name="message">The validation message.</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TElement> IsInt<T, TElement>(this IRuleBuilder<T, TElement> options, bool allowNull = false, string message = null) =>
            options.SetValidator(new IsIntValidator(allowNull, message));

        /// <summary>
        /// Validates the given property is convertable to a decimal value.
        /// </summary>
        /// <typeparam name="T">Type to validate.</typeparam>
        /// <typeparam name="TElement">Type of property to validate.</typeparam>
        /// <param name="options">Rule builder.</param>
        /// <param name="message">The validation message.</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TElement> IsDecimal<T, TElement>(this IRuleBuilder<T, TElement> options, bool allowNull = false, string message = null) =>
            options.SetValidator(new IsDecimalValidator(allowNull, message));

        /// <summary>
        /// Validates the given property is one of the given values.
        /// </summary>
        /// <typeparam name="T">Type to validate.</typeparam>
        /// <typeparam name="TElement">Type of property to validate.</typeparam>
        /// <param name="options">Rule builder.</param>
        /// <param name="values">The allowed values.</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TElement> IsOneOf<T, TElement>(this IRuleBuilder<T, TElement> options, params TElement[] values) =>
            options.SetValidator(new OneOfValidator<TElement>(null, values));

        /// <summary>
        /// Validates the given property is an alphanumeric string.
        /// </summary>
        /// <typeparam name="T">Type to validate.</typeparam>
        /// <typeparam name="TElement">Type of property to validate.</typeparam>
        /// <param name="options">Rule builder.</param>
        /// <param name="values">The allowed values.</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TElement> Alphanumeric<T, TElement>(this IRuleBuilder<T, TElement> options, string message = null) =>
            options.SetValidator(new IsAlphanumericValidator(message));

        /// <summary>
        /// Validates that the array has a length between the min and max values. Inclusive and exclusive respectively.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="options"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TElement[]> Length<T, TElement>(this IRuleBuilder<T, TElement[]> options, int min = int.MinValue, int max = int.MaxValue, string message = null) =>
            options.SetValidator(new ArrayLengthValidator(min, max, message));

    }
}