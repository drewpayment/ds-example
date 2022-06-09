using System;
using System.Collections.Generic;

namespace Dominion.Utility.Messaging
{
    /// <summary>
    /// Interface definition for a generic status message list.
    /// </summary>
    public interface IStatusMessageList : IEnumerable<IStatusMessage>
    {
        bool HasError { get; }
        void Add(IStatusMessage statusMessage);
        void Add(StatusMessageLevelType level, string message);
        void Add(StatusMessageLevelType level, string message, Exception sourceException);
        void Add(IEnumerable<IStatusMessage> statusMessages);
        string GetTextForAllMessages();
        string GetTextForAllMessages(StatusMessageLevelType minLevel);
    }
}