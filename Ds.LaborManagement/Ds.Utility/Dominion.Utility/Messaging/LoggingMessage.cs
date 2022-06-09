using System;

namespace Dominion.Utility.Messaging
{
    /// <summary>
    /// This is a container class for a status message that logs each message that is created.
    /// </summary>
    public class LoggingMessage : StatusMessage
    {
        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="level">The message level.</param>
        /// <param name="text">The message text.</param>
        public LoggingMessage(StatusMessageLevelType level, string text)
            : this(level, text, null)
        {
        }

        /// <summary>
        /// Constructor for status messages generated after catching an exception.
        /// </summary>
        /// <param name="level">The message level.</param>
        /// <param name="contextMessage">Message text that provides some context for the exception.</param>
        /// <param name="exception">The exception that inspired this status message.</param>
        public LoggingMessage(StatusMessageLevelType level, string contextMessage, Exception exception)
            : base(level, contextMessage, exception)
        {
            // todo: log the message.
        }
    }
}