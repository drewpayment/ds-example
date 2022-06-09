using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Dominion.Utility.Messaging
{
    /// <summary>
    /// This is a container class for a validation status message.
    /// </summary>
    public class ValidationStatusMessage : ValidationResult, IValidationStatusMessage, IStatusMessage
    {
        public const string DEFAULT_REQUIRED_VALUE_MESSAGE = "One or more required values are missing.";

        // IStatusMessage members
        private StatusMessageLevelType _level;
        private Exception _sourceException;

        // IValidationStatusMessage members
        private ValidationStatusMessageType _messageType;


        /// <summary>
        /// Create a new instance with the given text and members.
        /// </summary>
        /// <remarks>
        /// The message level is set to Error. Message type is set to General.
        /// </remarks>
        /// <param name="text">Message text.</param>
        /// <param name="messageType">Type of validation message.</param>
        /// <param name="memberNames">List of field names to which this message applies.</param>
        public ValidationStatusMessage(string text, IEnumerable<string> memberNames = null)
            : this(text, ValidationStatusMessageType.General, memberNames, StatusMessageLevelType.Error)
        {
        }

        /// <summary>
        /// Create a new instance with the given text, type and members.
        /// </summary>
        /// <remarks>
        /// The message level is set to Error.
        /// </remarks>
        /// <param name="text">Message text.</param>
        /// <param name="messageType">Type of validation message.</param>
        /// <param name="memberNames">List of field names to which this message applies.</param>
        public ValidationStatusMessage(string text, ValidationStatusMessageType messageType, 
            IEnumerable<string> memberNames = null)
            : this(text, messageType, memberNames, StatusMessageLevelType.Error)
        {
        }

        /// <summary>
        /// Fully initializing constructor.
        /// </summary>
        /// <param name="text">The message text.</param>
        /// <param name="messageType">The type of validation message.</param>
        /// <param name="memberNames">List of field names to which this message applies.</param>
        /// <param name="level">The message level.</param>
        /// <param name="sourceException">The exception that inspired this status message.</param>
        public ValidationStatusMessage(string text, 
            ValidationStatusMessageType messageType, 
            IEnumerable<string> memberNames, 
            StatusMessageLevelType level, 
            Exception sourceException = null)
            : base(text, memberNames != null ? memberNames : new List<string>())
        {
            _level = level;
            _sourceException = sourceException;
            _messageType = messageType;
        }

        #region IStatusMessage MEMBERS

        /// <summary>
        /// Get the status level for this message.
        /// </summary>
        StatusMessageLevelType IStatusMessage.Level
        {
            get { return _level; }
        }

        /// <summary>
        /// Get the text for this message.
        /// </summary>
        public string Text
        {
            get { return ErrorMessage; }
        }

        /// <summary>
        /// Get the source exception for this message. If this message was not the result of an
        /// exception, this value will be null.
        /// </summary>
        public Exception SourceException
        {
            get { return _sourceException; }
        }

        #endregion // IStatusMessage MEMBERS

        #region IValidationStatusMessage MEMBERS

        // note that ValidationResult handles MemberNames for us.

        /// <summary>
        /// The type of validation error.
        /// </summary>
        public ValidationStatusMessageType MessageType
        {
            get { return _messageType; }
        }

        #endregion // IValidationStatusMessage MEMBERS

        /// <summary>
        /// Override of the ToString() message to return what is essetially a serialized version of this object.
        /// </summary>
        /// <returns>The Text value for the message.</returns>
        public override string ToString()
        {
            // level and err msg.
            var fullText = new StringBuilder(_level.ToString() + ": " + this.ErrorMessage);

            // validation type and members/fields.
            fullText.Append(Environment.NewLine + "Validation Type: " + _messageType.ToString());
            if (this.MemberNames.Count() > 0)
            {
                fullText.Append(Environment.NewLine + "Field Names:");
                foreach (string fieldName in this.MemberNames)
                    fullText.Append(Environment.NewLine + "  " + fieldName);
            }

            // exception.
            if (_sourceException != null)
            {
                fullText.Append(Environment.NewLine + "Source Exception: " + _sourceException.Message);

#if DEBUG


// Add inner exception message(s) if available
                var innerException = _sourceException.InnerException;
                while (innerException != null)
                {
                    fullText.Append(Environment.NewLine + "Inner Exception: " + innerException.Message);
                    innerException = innerException.InnerException;
                }

                fullText.Append(string.Format("{0}Stack Trace: {1}", Environment.NewLine, _sourceException.StackTrace));
#endif
            }

            return fullText.ToString();
        }

        // ToString()
    } // class ValidationStatusMessage
}