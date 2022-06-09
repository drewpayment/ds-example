using System;

namespace Dominion.Utility.Messaging
{
    /// <summary>
    /// Available status message levels.
    /// </summary>
    public enum StatusMessageLevelType
    {
        // note that the integer value is important for determining relative significance.
        Debug = 0, 
        Info = 1, 
        Warn = 2, 
        Error = 3, 
        Fatal = 4
    }


    /// <summary>
    /// Interface definition for generic status messages.
    /// </summary>
    /// <remarks>
    /// Notice that this encourages the implementation of an immutable concrete class.
    /// </remarks>
    public interface IStatusMessage
    {
        StatusMessageLevelType Level { get; }
        string Text { get; }
        Exception SourceException { get; }
    }
}