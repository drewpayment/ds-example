using System.Collections.Generic;
using Dominion.Utility.Validation;

namespace Dominion.Utility.Messaging
{
    /// <summary>
    /// Object wrapper used to attach validation status messages to a given type. Typically used to attach messages to
    /// method return values.
    /// </summary>
    /// <typeparam name="T">Type of object the messages are being attached to.</typeparam>
    public class ValidationStatusMessenger<T> : IValidationObject
    {
        #region VARIABLES & PROPERTIES

        private ValidationStatusMessageList _validationMessages;

        /// <summary>
        /// Underlying object instance the messages are being attached to.
        /// </summary>
        public T Value { get; set; }

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Instantiates a new ValidationStatusMessenger object.
        /// </summary>
        public ValidationStatusMessenger()
        {
            Init();
        }

        /// <summary>
        /// Instantiates a new ValidationStatusMessenger object.
        /// </summary>
        /// <param name="initialValue">Value to initialize the object being wrapped to.</param>
        public ValidationStatusMessenger(T initialValue)
        {
            Value = initialValue;
            Init();
        }

        private void Init()
        {
            _validationMessages = new ValidationStatusMessageList();
        }

        #endregion

        #region IValidationObject Members

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
        #endregion // IValidationObject Members
    } // class ValidationStatusMessenger
}