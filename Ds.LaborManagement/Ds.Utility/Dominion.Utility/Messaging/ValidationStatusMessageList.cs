using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Dominion.Utility.Msg;
using Dominion.Utility.Msg.Specific;
using Dominion.Utility.OpResult;
using Dominion.Utility.Validation;

namespace Dominion.Utility.Messaging
{
    /// <summary>
    /// This is a container class for a list of status messages.
    /// </summary>
    [Serializable]
    public class ValidationStatusMessageList : List<IValidationStatusMessage>, IValidationStatusMessageList
    {
        #region IValidationStatusMessageList IMPLEMENTATION

        /// <summary>
        /// Indicate whether the list includes any error-level (or higher) messages.
        /// </summary>
        public bool HasError
        {
            get { return this.FindAll(x => (int) x.Level >= (int) StatusMessageLevelType.Error).Count > 0; }
        }

        /// <summary>
        /// Indicate that there are currently no error messages detected.
        /// </summary>
        public bool HasNoError
        {
            get { return !HasError; }
        }

        /// <summary>
        /// Add a new validation status message with the given message and message type.
        /// </summary>
        /// <remarks>
        /// The message level is set to Error.
        /// </remarks>
        /// <param name="text">Message text.</param>
        /// <param name="messageType">Type of validation message.</param>
        /// <param name="memberNames">List of field names to which this message applies.</param>
        public void Add(string text, ValidationStatusMessageType messageType, IEnumerable<string> memberNames = null)
        {
            this.Add(text, messageType, memberNames, StatusMessageLevelType.Error, null);
        }

        /// <summary>
        /// Add the given validation errors to the list.
        /// </summary>
        /// <remarks>
        /// If any of the list items implement IValidationStatusMessage, those objects are typcasted
        /// then added to the list.
        /// </remarks>
        /// <param name="errors">The errors to add.</param>
        public void Add(IEnumerable<ValidationResult> errors)
        {
            foreach (var error in errors)
            {
                if (error is IValidationStatusMessage)
                    this.Add(error as IValidationStatusMessage);
                else
                    this.Add(new ValidationStatusMessage(error.ErrorMessage, error.MemberNames));
            }
        }

        /// <summary>
        /// Add a new validation status message with the given info.
        /// </summary>
        /// <param name="text">Message text.</param>
        /// <param name="messageType">Type of validation message.</param>
        /// <param name="memberNames">List of field names to which this message applies.</param>
        /// <param name="level">The level of the new message.</param>
        /// <param name="exception">The exception that inspired this status message.</param>
        public void Add(
            string text, 
            ValidationStatusMessageType messageType, 
            IEnumerable<string> memberNames, 
            StatusMessageLevelType level, 
            Exception sourceException)
        {
            this.Add(new ValidationStatusMessage(text, messageType, memberNames, level, sourceException));
        }

        /// <summary>
        /// Add the messages in the given list to this one.
        /// </summary>
        /// <param name="statusMessages">The messages to add.</param>
        public void Add(IEnumerable<IValidationStatusMessage> statusMessages)
        {
            this.AddRange(statusMessages);
        }

        /// <summary>
        /// Convert all the messages from the op result to validation messages.
        /// </summary>
        /// <param name="messages"></param>
        public void Add(IEnumerable<IMsgSimple> messages)
        {
            //ValidationStatusMessage
            foreach (var msg in messages)
            {
                var vm = msg.ToValidationStatusMsg();
                this.Add(vm);
            }
        }

        /// <summary>
        /// Add the messages in the given list to this one.
        /// </summary>
        /// <param name="statusMessages">The messages to add.</param>
        public void Add(IEnumerable<IStatusMessage> statusMessages)
        {
            foreach (var statusMessage in statusMessages)
            {
                this.Add(new ValidationStatusMessage(
                    statusMessage.Text, 
                    ValidationStatusMessageType.General, 
                    null, statusMessage.Level, 
                    statusMessage.SourceException));
            }
        }

        /// <summary>
        /// Generate a text string that includes all messages.
        /// </summary>
        /// <returns>A text string that includes all messages.</returns>
        public string GetTextForAllMessages()
        {
            StringBuilder text = new StringBuilder();

            foreach (IStatusMessage message in this)
            {
                text.Append(Environment.NewLine + message.ToString());
            }

            return text.ToString();
        }

        // GetTextForAllMessages()

        /// <summary>
        /// Get this list as a set of ValidationResult objects.
        /// </summary>
        /// <returns>This list as a set of ValidationResult objects.</returns>
        public IEnumerable<ValidationResult> ToValidationResultList()
        {
            return
                from result in this
                select new ValidationResult(result.Text, result.MemberNames);
        }

        #endregion // IValidationStatusMessageList IMPLEMENTATION
    } // class ValidationStatusMessageList
}