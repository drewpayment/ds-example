using System.Collections.Generic;
using Dominion.Utility.Validation;
using Dominion.Utility.Validation.FluentValidate.PropertyValidate;

namespace Dominion.Utility.Msg.Specific
{
    public interface IPropertyValidatorMsg
    {
        /// <summary>
        /// True if there are no errors.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// A list of property validation message objects.
        /// </summary>
        IEnumerable<PropertyValidationMsg> PropertyErrors { get; }

        /// <summary>
        /// A custom object that may be associated with the errors.
        /// </summary>
        object CustomObject { get; }
    }
}