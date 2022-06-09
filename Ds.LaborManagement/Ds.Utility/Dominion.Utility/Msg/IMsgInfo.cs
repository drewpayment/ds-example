using System;
using Dominion.Utility.Msg.Identifiers;

namespace Dominion.Utility.Msg
{
    /// <summary>
    /// This is defines the most basic functionality needed to get a message.
    /// This shouldn't be used to build a message class.
    /// It should be used to get to a mesaage or messages built from a class that contains data used to build a message. 
    /// </summary>
    public interface IMsgSimple
    {
        /// <summary>
        /// Gets the message.
        /// </summary>
        string Msg { get; }

        /// <summary>
        /// The message code.
        /// Give meaning to your messages via enums why don't ya.
        /// </summary>
        int Code { get; set; }

        /// <summary>
        /// The message priority level (enum).
        /// </summary>
        MsgLevels Level { get; set; }

        /// <summary>
        /// The message priority level enum string.
        /// </summary>
        string LevelString { get; }
    }

    /// <summary>
    /// Functionality for defining a class with a specific set of data.
    /// </summary>
    /// <typeparam name="TMsgData">The type of the class your defining.</typeparam>
    public interface IMsg<TMsgData> : IMsgSimple
    {
        /// <summary>
        /// Define a 'custom' function for turning the msg data into a string message.
        /// This is primarily an idea for outside parties defining string messages that aren't default to our API.
        /// </summary>
        Func<TMsgData, string> AltMsg { get; set; }
    }
}