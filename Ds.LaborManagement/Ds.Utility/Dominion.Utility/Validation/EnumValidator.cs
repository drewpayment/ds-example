using System;

using AutoMapper.Internal;

using Dominion.Utility.ExtensionMethods;

using FluentValidation.Validators;

namespace Dominion.Utility.Validation
{
    /// <summary>
    /// Validates a property's value is found within the given enum.
    /// </summary>
    public class EnumValidator : PropertyValidator, IPropertyValidator
    {
        /// <summary>
        /// Instantiates a new EnumValidator with the default error message.
        /// </summary>
        public EnumValidator()
            : base("{PropertyValue} is an invalid enum value.")
        {
        }

        /// <summary>
        /// Instantiates a new EnumValidator with the specified error message.
        /// </summary>
        /// <param name="errorMessage"></param>
        public EnumValidator(string errorMessage)
            : base(errorMessage)
        {
        }

        /// <summary>
        /// Validates the property within the current context.
        /// </summary>
        /// <param name="context">Context containing the current property value.</param>
        /// <returns>True: Valid enum value.  False: Invalid enum value.</returns>
        protected override bool IsValid(PropertyValidatorContext context)
        {
            // Ensure the property is an Enum and the current value is contained in the 
            // available enumeration values.
            if (!context.Rule.TypeToValidate.GetUnderlyingType().IsEnum ||
                !Enum.IsDefined(context.Rule.TypeToValidate.GetUnderlyingType(), context.PropertyValue))
            {
                return false;
            }

            return true;
        }
    }
}