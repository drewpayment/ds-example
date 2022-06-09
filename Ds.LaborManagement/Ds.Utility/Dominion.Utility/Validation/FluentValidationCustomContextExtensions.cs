using System.Collections.Generic;
using System.Linq;
using Dominion.Utility.Messaging;
using FluentValidation;
using FluentValidationFailure = FluentValidation.Results.ValidationFailure;
using FluentValidationResults = FluentValidation.Results.ValidationResult;


namespace Dominion.Utility.Validation
{
    /// <summary>
    /// This class contains fluent validation extension methods relating to the FluentValidationCustomContext class.
    /// </summary>
    /// <remarks>
    /// This approach is based on the codeplex.com discussion that can be found here:
    /// http://fluentvalidation.codeplex.com/discussions/355890
    /// </remarks>
    public static class FluentValidationCustomContextExtensions
    {
        /// <summary>
        /// Extension method that converts a set of fluent validation results to a set of validation status messages.
        /// </summary>
        /// <param name="fluentResults">Set of fluent validation results to be converted.</param>
        /// <returns>Set of validation status messages based on a set of fluent validation results.</returns>
        public static IEnumerable<ValidationStatusMessage> AsValidationStatusMessages(
            this FluentValidationResults fluentResults)
        {
            // if there are no errors, bail now.
            if (fluentResults.IsValid)
                return Enumerable.Empty<ValidationStatusMessage>();

            // convert the errors into a list of ValidationStatusMessage
            IEnumerable<ValidationStatusMessage> rawStatusMessages = new List<ValidationStatusMessage>();
            rawStatusMessages = fluentResults.Errors.Select(result => { return result.AsValidationStatusMessage(); });

            // combine 'required value' messages and set custom text for all that have a non-general message type.
            var requiredMessage = new ValidationStatusMessage(ValidationStatusMessage.DEFAULT_REQUIRED_VALUE_MESSAGE);
            var requiredMembers = new List<string>();
            var statusMessages = new List<ValidationStatusMessage>();
            foreach (var message in rawStatusMessages)
            {
                if (message.MessageType == ValidationStatusMessageType.Required)
                    requiredMembers.AddRange(message.MemberNames);
                else
                    statusMessages.Add(message);
            }

            if (requiredMembers.Count > 0)
            {
                statusMessages.Insert(0, new ValidationStatusMessage(
                    ValidationStatusMessage.DEFAULT_REQUIRED_VALUE_MESSAGE, 
                    ValidationStatusMessageType.Required, 
                    requiredMembers, 
                    StatusMessageLevelType.Error));
            }

            return statusMessages;
        }

        // AsValidationStatusMessages()

        /// <summary>
        /// Extension method that converts a fluent validation failure to a validation status messages.
        /// </summary>
        /// <param name="fluentError">Fluent validation failure to be converted.</param>
        /// <returns>Validation status message based on the given fluent validation failure.</returns>
        public static ValidationStatusMessage AsValidationStatusMessage(
            this FluentValidationFailure fluentError)
        {
            // get the name of the entity property whose value is invalid
            var members = new List<string>();
            members.Add(fluentError.PropertyName);

            // get the fluent error custom context info.
            var validationType = ValidationStatusMessageType.General;
            var level = StatusMessageLevelType.Error;
            var context = fluentError.CustomState as FluentValidationCustomContext;
            if (context != null)
            {
                validationType = context.ValidationType;
                level = context.Level;
                if (context.AdditionalPropertyNames != null)
                    members.AddRange(context.AdditionalPropertyNames);
            }

            // get the message text
            var messageText = string.Empty;
            switch (validationType)
            {
                case ValidationStatusMessageType.Required:
                    messageText = ValidationStatusMessage.DEFAULT_REQUIRED_VALUE_MESSAGE;
                    break;
                case ValidationStatusMessageType.OutOfRange:
                case ValidationStatusMessageType.Incompatible:
                case ValidationStatusMessageType.ActionNotAllowed:
                case ValidationStatusMessageType.General:
                default:
                    messageText = fluentError.ErrorMessage;
                    break;
            }

            return new ValidationStatusMessage(messageText, validationType, members, level);
        }

        // AsValidationStatusMessage()

        /// <summary>
        /// Fluent validation extension method to enable a FluentValidationCustomContext object to be
        /// associated with a fluent validation rule.
        /// </summary>
        /// <typeparam name="T">Target entity type.</typeparam>
        /// <typeparam name="TProperty">Property being evaluated.</typeparam>
        /// <param name="rule">Fluent validation rule builder.</param>
        /// <param name="validationType">Type of validation being performed.</param>
        /// <param name="level">Severity of breaking the rule.</param>
        /// <param name="additionalPropertyNames">Properties associated with the rule, in addition to the target 
        /// validation property.</param>
        /// <returns>The enhanced fluent validation rule.</returns>
        public static IRuleBuilderOptions<T, TProperty> WithValidationContext<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule, 
            ValidationStatusMessageType validationType, 
            StatusMessageLevelType level, 
            IEnumerable<string> additionalPropertyNames = null)
            where T : class
        {
            rule.WithState(x => new FluentValidationCustomContext(validationType, level, additionalPropertyNames));
            return rule;
        }

        /// <summary>
        /// Fluent validation extension method to enable a FluentValidationCustomContext object to be
        /// associated with a fluent validation rule.
        /// </summary>
        /// <typeparam name="T">Target entity type.</typeparam>
        /// <typeparam name="TProperty">Property being evaluated.</typeparam>
        /// <param name="rule">Fluent validation rule builder.</param>
        /// <returns>The enhanced fluent validation rule.</returns>
        public static IRuleBuilderOptions<T, TProperty> WithValidationContext<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule)
            where T : class
        {
            return rule.WithValidationContext(ValidationStatusMessageType.General, StatusMessageLevelType.Error);
        }

        /// <summary>
        /// Fluent validation extension method to enable a FluentValidationCustomContext object to be
        /// associated with a fluent validation rule.
        /// </summary>
        /// <typeparam name="T">Target entity type.</typeparam>
        /// <typeparam name="TProperty">Property being evaluated.</typeparam>
        /// <param name="rule">Fluent validation rule builder.</param>
        /// <param name="additionalProperties">Properties associated with the rule, in addition to the target 
        /// validation property.</param>
        /// <returns>The enhanced fluent validation rule.</returns>
        public static IRuleBuilderOptions<T, TProperty> WithIncompatibleContext<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule, 
            IEnumerable<string> additionalPropertyNames = null)
            where T : class
        {
            return rule.WithValidationContext(
                ValidationStatusMessageType.Incompatible, 
                StatusMessageLevelType.Error, 
                additionalPropertyNames);
        }

        /// <summary>
        /// Fluent validation extension method to enable a FluentValidationCustomContext object to be
        /// associated with a fluent validation rule.
        /// </summary>
        /// <typeparam name="T">Target entity type.</typeparam>
        /// <typeparam name="TProperty">Property being evaluated.</typeparam>
        /// <param name="rule">Fluent validation rule builder.</param>
        /// <returns>The enhanced fluent validation rule.</returns>
        public static IRuleBuilderOptions<T, TProperty> WithOutOfRangeContext<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule)
            where T : class
        {
            return rule.WithValidationContext(ValidationStatusMessageType.OutOfRange, StatusMessageLevelType.Error);
        }

        /// <summary>
        /// Fluent validation extension method to enable a FluentValidationCustomContext object to be
        /// associated with a fluent validation rule.
        /// </summary>
        /// <typeparam name="T">Target entity type.</typeparam>
        /// <typeparam name="TProperty">Property being evaluated.</typeparam>
        /// <param name="rule">Fluent validation rule builder.</param>
        /// <returns>The enhanced fluent validation rule.</returns>
        public static IRuleBuilderOptions<T, TProperty> WithRequiredContext<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule)
            where T : class
        {
            return rule.WithValidationContext(ValidationStatusMessageType.Required, StatusMessageLevelType.Error);
        }

        /// <summary>
        /// Fluent validation extension method to enable a FluentValidationCustomContext object to be
        /// associated with a fluent validation rule.
        /// </summary>
        /// <typeparam name="T">Target entity type.</typeparam>
        /// <typeparam name="TProperty">Property being evaluated.</typeparam>
        /// <param name="rule">Fluent validation rule builder.</param>
        /// <returns>The enhanced fluent validation rule.</returns>
        public static IRuleBuilderOptions<T, TProperty> WithInvalidFormatContext<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule)
            where T : class
        {
            return rule.WithValidationContext(ValidationStatusMessageType.InvalidFormat, StatusMessageLevelType.Error);
        }
    } // class FluentValidationCustomContextExtensions
}