using System;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Msg.Identifiers;

namespace Dominion.Utility.Msg.Specific
{
    /// <summary>
    /// A class for gathering the exception when you only want the basic exception functionality available.
    /// </summary>
    public class GenericMsg : MsgBase<GenericMsg>
    {
        /// <summary>
        /// The exception data.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Constructor.
        /// By default this is a fatal error.
        /// </summary>
        /// <param name="ex">Exception object.</param>
        public GenericMsg(string msg) 
            : this(msg, MsgLevels.Error) 
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="msgLevel"></param>
        public GenericMsg(string msg, MsgLevels msgLevel)
            : base(msgLevel, MsgCodes.BasicException)
        {
            Message = msg;
        }

        /// <summary>
        /// Method that builds the default message.
        /// </summary>
        /// <returns></returns>
        protected override string BuildMsg()
        {
            return Message;
        }
    }
}