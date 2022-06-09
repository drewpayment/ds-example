using System.Collections.Generic;
using Dominion.Utility.Msg.Identifiers;
using Dominion.Utility.Validation;
using Dominion.Utility.Validation.FluentValidate.PropertyValidate;

namespace Dominion.Utility.Msg.Specific
{
    /// <summary>
    /// Holds 0-n property validation messages.
    /// </summary>
    public class PropertyValidationContainerMsg : MsgBase<PropertyValidationContainerMsg>
    {
        /// <summary>
        /// The entity/object that was validated.
        /// </summary>
        public object Entity { get; set; }

        /// <summary>
        /// A list of property validation message objects.
        /// </summary>
        public IList<PropertyValidationMsg> PropertyErrors { get; set; }

        /// <summary>
        /// Constructor.
        /// More than likely will only hold validation errors which are considered fatal.
        /// </summary>
        public PropertyValidationContainerMsg()
            : base(MsgLevels.Fatal, MsgCodes.EntityFrameworkValidation)
        {
            PropertyErrors = new List<PropertyValidationMsg>();
        }
    }
}