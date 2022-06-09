using System.Collections.Generic;
using System.Text;
using Dominion.Utility.Msg.Identifiers;

namespace Dominion.Utility.Msg
{
    /// <summary>
    /// A base class that used by classes that group messages under a parent type.
    /// </summary>
    /// <typeparam name="TMsgData">The type of the class your defining.</typeparam>
    public class MsgList : MsgBase<MsgList>
    {
        /// <summary>
        /// The list of messages in this container.
        /// </summary>
        public IList<IMsgSimple> Messages { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="msgLevel">The messages level.</param>
        /// <param name="msgCode">The messages code.</param>
        protected MsgList(MsgLevels msgLevel, int msgCode)
            : base(msgLevel, msgCode)
        {
            Messages = new List<IMsgSimple>();
        }

        /// <summary>
        /// Returns all the messages combined in string builder.
        /// So in the end you get one string; MAGIC.
        /// </summary>
        public override string Msg
        {
            get
            {
                var sb = new StringBuilder();
                foreach(var msgSimple in Messages)
                {
                    sb.AppendLine(msgSimple.Msg).AppendLine();
                }

                return sb.ToString();
            }
        }
    }
}