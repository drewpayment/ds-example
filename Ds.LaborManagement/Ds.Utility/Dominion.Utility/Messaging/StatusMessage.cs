using System;

namespace Dominion.Utility.Messaging
{
    /// <summary>
    /// This is a container class for a status message.
    /// </summary>
    public class StatusMessage : IStatusMessage
    {
        private StatusMessageLevelType _level;
        private string _text;
        private Exception _sourceException;


        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="level">The message level.</param>
        /// <param name="text">The message text.</param>
        public StatusMessage(StatusMessageLevelType level, string text)
            : this(level, text, null)
        {
        }

        /// <summary>
        /// Constructor for status messages generated after catching an exception.
        /// </summary>
        /// <param name="level">The message level.</param>
        /// <param name="contextMessage">Message text that provides some context for the exception.</param>
        /// <param name="exception">The exception that inspired this status message.</param>
        public StatusMessage(StatusMessageLevelType level, string contextMessage, Exception exception)
        {
            _level = level;
            _text = contextMessage;
            _sourceException = exception;
        }

        #region IStatusMessage Members

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
        public virtual string Text
        {
            get { return _text; }
        }

        /// <summary>
        /// Get the source exception for this message. If this message was not the result of an
        /// exception, this value will be null.
        /// </summary>
        public Exception SourceException
        {
            get { return _sourceException; }
        }

        #endregion // IStatusMessage Members

        /// <summary>
        /// Override of the ToString() message to return the message's text value.
        /// </summary>
        /// <returns>The Text value for the message.</returns>
        public override string ToString()
        {
            var fullText = _level.ToString() + ": " + _text;

            if (_sourceException != null)
            {
                fullText += Environment.NewLine + "Source Exception: " + _sourceException.Message;

#if DEBUG


// Add inner exception message(s) if available
                var innerException = _sourceException.InnerException;
                while (innerException != null)
                {
                    fullText += Environment.NewLine + "Inner Exception: " + innerException.Message;
                    innerException = innerException.InnerException;
                }

                fullText = string.Format("{0}{1}Stack Trace: {2}", fullText, Environment.NewLine, 
                    _sourceException.StackTrace);
#endif
            }

            return fullText;
        }

    } 
}