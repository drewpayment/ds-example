using Dominion.Utility.Msg.Identifiers;

namespace Dominion.Utility.Msg
{
    /// <summary>
    /// Condensed version of a business message object.
    /// For transferring to a client.
    /// Condensed so all the non-message data doesn't get transferred client which would contain much more data.
    /// The non-condensed data should be logged separately if needed. The client only needs the message data.
    /// </summary>
    public class MsgDto
    {
        /// <summary>
        /// The original message.
        /// </summary>
        private readonly IMsgSimple _msg;

        /// <summary>
        /// The message text.
        /// </summary>
        public string Msg
        {
            get { return _msg.Msg; }
        }

        /// <summary>
        /// The message code.
        /// </summary>
        public int Code
        {
            get { return _msg.Code; }
        }

        /// <summary>
        /// The message level.
        /// </summary>
        public MsgLevels Level
        {
            get { return _msg.Level; }
        }

        /// <summary>
        /// The message level string.
        /// </summary>
        public string LevelString
        {
            get { return _msg.LevelString; }
        }

        /// <summary>
        /// The message level string.
        /// </summary>
        public string TypeString
        {
            get; private set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="msg"></param>
        public MsgDto(IMsgSimple msg)
        {
            _msg = msg;
            TypeString = _msg.GetType().Name;
        }
    }
}