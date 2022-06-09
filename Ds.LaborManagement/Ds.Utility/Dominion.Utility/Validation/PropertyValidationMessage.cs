using Dominion.Utility.Msg;
using Dominion.Utility.Msg.Identifiers;

namespace Dominion.Utility.Validation
{
    /// <summary>
    /// Holds property validation information.
    /// More than likely will only hold validation errors which are considered fatal.
    /// </summary>
    public class PropertyValidationMsg : MsgBase<PropertyValidationMsg>
    {
        /// <summary>
        /// The property name on the object being validated.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// The message associated with that validation.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The value validated.
        /// This can be an entire collection.
        /// </summary>
        public object AttemptedValue { get; set; }

        /// <summary>
        /// A validation state.
        /// </summary>
        public EntityValidationTypes CustomState { get; set; }

        /// <summary>
        /// Constructor.
        /// By default this is considered a fatal error.
        /// </summary>
        public PropertyValidationMsg()
            : base(MsgLevels.Fatal, MsgCodes.EntityValidation)
        {
        }

        protected override string BuildMsg()
        {
            return Message;
        }
    }
}