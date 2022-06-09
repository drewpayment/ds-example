using Dominion.Utility.Msg;
using Dominion.Utility.Msg.Identifiers;

namespace Dominion.Utility.Security.Msg
{
    /// <summary>
    /// Base object for security related messages.
    /// </summary>
    /// <typeparam name="TMsgType">Type of security message.</typeparam>
    public abstract class SecurityMsg<TMsgType> : MsgBase<TMsgType> where TMsgType : class, IMsg<TMsgType>
    {
        /// <summary>
        /// Enumeration type of security message.
        /// </summary>
        public SecurityMessageType MessageType
        {
            get { return (SecurityMessageType) Code; }
        }

        /// <summary>
        /// Instantiates a new SecurityMsg.
        /// </summary>
        /// <param name="level">Message severity level.</param>
        /// <param name="type">Type of security message.</param>
        protected SecurityMsg(MsgLevels level, SecurityMessageType type) : base(level, (int) type)
        {
        }
    }
}