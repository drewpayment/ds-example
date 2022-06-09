using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dominion.Utility.Msg;

namespace Dominion.Utility.Messaging
{
    /// <summary>
    /// Interface definition for a validation status message list.
    /// </summary>
    public interface IValidationStatusMessageList : IEnumerable<IValidationStatusMessage>
    {
        /// <summary>
        /// Determine whether this list includes messages that are error level or higher.
        /// </summary>
        bool HasError { get; }

        /// <summary>
        /// Indicate that there are currently no error messages detected.
        /// </summary>
        bool HasNoError { get; }

        /// <summary>
        /// Add the given message to the list.
        /// </summary>
        /// <param name="statusMessage">The message to add.</param>
        void Add(IValidationStatusMessage statusMessages);

        /// <summary>
        /// Add the given validation errors to the list.
        /// </summary>
        /// <param name="errors">The errors to add.</param>
        void Add(IEnumerable<ValidationResult> errors);

        /// <summary>
        /// Convert all the messages from the op result to validation messages.
        /// </summary>
        /// <param name="messages"></param>
        void Add(IEnumerable<IMsgSimple> messages);

        /// <summary>
        /// Add a new validation status message with the given message and message type.
        /// </summary>
        /// <remarks>
        /// The message level is set to Error.
        /// </remarks>
        /// <param name="text">Message text.</param>
        /// <param name="messageType">Type of validation message.</param>
        /// <param name="fieldNames">List of field names to which this message applies.</param>
        void Add(string text, ValidationStatusMessageType messageType, IEnumerable<string> fieldNames = null);

        /// <summary>
        /// Add a new validation status message with the given info.
        /// </summary>
        /// <param name="text">Message text.</param>
        /// <param name="messageType">Type of validation message.</param>
        /// <param name="fieldNames">List of field names to which this message applies.</param>
        /// <param name="level">The level of the new message.</param>
        /// <param name="exception">The exception that inspired this status message.</param>
        void Add(string text, 
            ValidationStatusMessageType messageType, 
            IEnumerable<string> fieldNames, 
            StatusMessageLevelType level, 
            Exception sourceException);

        /// <summary>
        /// Add the messages in the given list to this one.
        /// </summary>
        /// <param name="statusMessages">The messages to add.</param>
        void Add(IEnumerable<IValidationStatusMessage> statusMessages);

        /// <summary>
        /// Add the messages in the given list to this one.
        /// </summary>
        /// <param name="statusMessages">The messages to add.</param>
        void Add(IEnumerable<IStatusMessage> statusMessages);

        /// <summary>
        /// Generate a text string that includes all messages.
        /// </summary>
        /// <returns>A text string that includes all messages.</returns>
        string GetTextForAllMessages();

        /// <summary>
        /// Get this list as a set of ValidationResult objects.
        /// </summary>
        /// <returns>This list as a set of ValidationResult objects.</returns>
        IEnumerable<ValidationResult> ToValidationResultList();
    }
}