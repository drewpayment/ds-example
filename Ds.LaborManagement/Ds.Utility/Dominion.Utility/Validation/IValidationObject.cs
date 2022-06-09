using System.Collections.Generic;
using Dominion.Utility.Messaging;

namespace Dominion.Utility.Validation
{
    /// <summary>
    /// Interface for objects that report the validity of their own property values.
    /// </summary>
    /// <remarks>
    /// This interface is primarily intended to be used by data transfer objects (DTOs)
    /// for reporting validation errors after an application-level action is performed.
    /// </remarks>
    public interface IValidationObject
    {
        /// <summary>
        /// Indicate whether this object is valid.
        /// </summary>
        bool IsValid { get; }


        /// <summary>
        /// Get the list of validation errors for this object.
        /// </summary>
        IValidationStatusMessageList ValidationMessages { get; }


        /// <summary>
        /// Add a new error to the ValidationErrors list.
        /// </summary>
        /// <param name="messageType">Type of validation message.</param>
        /// <param name="memberName">Name of the field that relates to the error.</param>
        void AddValidationError(string text, 
            ValidationStatusMessageType messageType = ValidationStatusMessageType.General, 
            IEnumerable<string> memberNames = null);

        /// <summary>
        /// Add a new error to the ValidationErrors list.
        /// </summary>
        /// <param name="message">The validation message to add.</param>
        void AddValidationMessage(IValidationStatusMessage message);

        /// <summary>
        /// Add the given errors to the ValidationErrors list.
        /// </summary>
        /// <param name="messages">The validation messages to add.</param>
        void AddValidationMessages(IEnumerable<IValidationStatusMessage> messages);
        void ClearValidationMessages();
    } // interface IValidationObject
}