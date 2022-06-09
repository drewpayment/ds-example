using System.Collections.Generic;
using Dominion.Utility.Messaging;

namespace Dominion.Utility.Validation
{
    /// <summary>
    /// This class, along with associated extention methods, allows validation message type and severity to be
    /// associated with a fluent validation rule.
    /// </summary>
    /// <remarks>
    /// This approach is based on the codeplex.com discussion that can be found here:
    /// http://fluentvalidation.codeplex.com/discussions/355890
    /// </remarks>
    public class FluentValidationCustomContext
    {
        /// <summary>
        /// Get the properties associated with the rule, in addition to the target validation property.
        /// </summary>
        public IEnumerable<string> AdditionalPropertyNames { get; private set; }

        /// <summary>
        /// Get the severity of the validation message.
        /// </summary>
        public StatusMessageLevelType Level { get; private set; }

        /// <summary>
        /// Get the type of validation message. ie: Required, OutOfRange, etc.
        /// </summary>
        public ValidationStatusMessageType ValidationType { get; private set; }

        /// <summary>
        /// Primary initializing constructor.
        /// </summary>
        /// <param name="validationType">The type of validation message.</param>
        /// <param name="level">The severity of the validation message.</param>
        /// <param name="additionalPropertyNames">Additional properties, if any, that are associated with the rule.</param>
        public FluentValidationCustomContext(
            ValidationStatusMessageType validationType, 
            StatusMessageLevelType level, 
            IEnumerable<string> additionalPropertyNames = null)
        {
            this.ValidationType = validationType;
            this.Level = level;
            this.AdditionalPropertyNames = additionalPropertyNames;
        }
    } // class FluentValidationCustomContext
}