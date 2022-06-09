using System;
using System.Collections.Generic;
using System.Text;

namespace Dominion.Utility.Messaging
{
    /// <summary>
    /// This is a container class for a list of status messages.
    /// </summary>
    [Serializable]
    public class StatusMessageList : List<IStatusMessage>, IStatusMessageList
    {
        #region IStatusMessageList IMPLEMENTATION

        /// <summary>
        /// Indicate whether the list includes any error-level (or higher) messages.
        /// </summary>
        public bool HasError
        {
            get
            {
                bool hasError = false;
                foreach (IStatusMessage message in this)
                {
                    if ((int) message.Level >= (int) StatusMessageLevelType.Error)
                    {
                        hasError = true;
                        break;
                    }
                }

                return hasError;
            }
        }


        /// <summary>
        /// Add a new message with the given level and text.
        /// </summary>
        /// <param name="level">The level of the new message.</param>
        /// <param name="text">The text of the new message</param>
        public void Add(StatusMessageLevelType level, string text)
        {
            this.Add(new StatusMessage(level, text));
        }


        /// <summary>
        /// Add a new message with the given level, text and exception.
        /// </summary>
        /// <param name="level">The level of the new message.</param>
        /// <param name="contextMessage">Message text that provides some context for the exception.</param>
        /// <param name="exception">The exception that inspired this status message.</param>
        public void Add(StatusMessageLevelType level, string contextMessage, Exception sourceException)
        {
            this.Add(new StatusMessage(level, contextMessage, sourceException));
        }


        /// <summary>
        /// Add all messages from the given list this this one.
        /// </summary>
        /// <param name="statusMessages">The list from which all messages are copied.</param>
        public void Add(IEnumerable<IStatusMessage> statusMessages)
        {
            this.AddRange(statusMessages);
        }


        /// <summary>
        /// Generate a text string that includes all messages at and above the given level.
        /// </summary>
        /// <param name="minLevel">Minimum level for which to include messages.</param>
        /// <returns>A text string that includes all messages.</returns>
        public string GetTextForAllMessages(StatusMessageLevelType minLevel)
        {
            StringBuilder text = new StringBuilder();

            foreach (IStatusMessage message in this)
            {
                if ((int) message.Level >= (int) minLevel)
                    text.Append(Environment.NewLine + message.ToString());
            }

            return text.ToString();
        }

        // GetTextForAllMessages()
        #endregion // IStatusMessageList IMPLEMENTATION

        /// <summary>
        /// Generate a text string that includes all messages.
        /// </summary>
        /// <returns>A text string that includes all messages.</returns>
        public string GetTextForAllMessages()
        {
            return GetTextForAllMessages(StatusMessageLevelType.Debug);
        }
    } // class StatusMessageList
}