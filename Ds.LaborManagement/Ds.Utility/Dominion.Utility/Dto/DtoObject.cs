using System;
using System.Collections.Generic;
using Dominion.Utility.Messaging;
using Dominion.Utility.Security;
using Dominion.Utility.Validation;

namespace Dominion.Utility.Dto
{
    /// <summary>
    /// Abstract base class for all API-level data transfer objects (DTOs). This class manages validation
    /// errors and related properties.
    /// </summary>
    [Serializable]
    public abstract class DtoObject : IValidationObject
    {
        /// <summary>
        /// Holds validation messages.
        /// </summary>
        protected ValidationStatusMessageList _validationMessages;

        /// <summary>
        /// Information about the type of edit capabilies a user has.
        /// </summary>
        public EditPermissions EditPermission { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DtoObject()
        {
            _validationMessages = new ValidationStatusMessageList();
            EditPermission = EditPermissions.NotDefined;
        }

        // -----------------------------------------------------------------------------------
        #region IValidatableObject Members

        /// <summary>
        /// Indicate whether this object is valid.
        /// </summary>
        /// <remarks>
        /// Note that this property is populated after an attempt is made to save the values 
        /// via a app object action.
        /// </remarks>
        public bool IsValid
        {
            get { return ! _validationMessages.HasError; }
        }


        /// <summary>
        /// Get the list of validation errors for this object.
        /// </summary>
        public IValidationStatusMessageList ValidationMessages
        {
            get { return _validationMessages; }
        }


        /// <summary>
        /// Add a new error to the ValidationErrors list.
        /// </summary>
        /// <param name="text">Message text that describes the error.</param>
        /// <param name="messageType">Type of validation message.</param>
        /// <param name="memberName">Name of the field that relates to the error.</param>
        public void AddValidationError(string text, 
            ValidationStatusMessageType messageType = ValidationStatusMessageType.General, 
            IEnumerable<string> memberNames = null)
        {
            _validationMessages.Add(text, messageType, memberNames);
        }

        /// <summary>
        /// Add the given message to the ValidationMessages list.
        /// </summary>
        /// <param name="message">The validation message to add.</param>
        public void AddValidationMessage(IValidationStatusMessage message)
        {
            _validationMessages.Add(message);
        }

        /// <summary>
        /// Add the given messages to the ValidationMessages list.
        /// </summary>
        /// <param name="messages">The validation messages to add.</param>
        public void AddValidationMessages(IEnumerable<IValidationStatusMessage> messages)
        {
            _validationMessages.AddRange(messages);
        }

        public void ClearValidationMessages()
        {
            _validationMessages.Clear();
        }
        #endregion // IValidatableObject Members
    }
}