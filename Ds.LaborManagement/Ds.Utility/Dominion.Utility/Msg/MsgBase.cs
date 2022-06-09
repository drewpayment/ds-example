using System;
using Dominion.Utility.Msg.Identifiers;

namespace Dominion.Utility.Msg
{
    /// <summary>
    /// All classes that contain data for message or messages should derive from this base class.
    /// </summary>
    /// <typeparam name="TMsgData">The type of the class your defining.</typeparam>
    public abstract class MsgBase<TMsgData> : IMsg<TMsgData>
        where TMsgData : class, IMsg<TMsgData>
    {
        #region Variables and Properties

        /// <summary>
        /// The msg.
        /// </summary>
        public virtual string Msg
        {
            get { return AltMsg != null ? AltMsg(this as TMsgData) : BuildMsg(); }
        }

        /// <summary>
        /// The message code.
        /// Give meaning to your messages via enums why don't ya.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// The message priority level.
        /// </summary>    
        public MsgLevels Level { get; set; }

        /// <summary>
        /// The message priority level enum string.
        /// </summary>
        public string LevelString
        {
            get { return Level.ToString(); }
        }

        /// <summary>
        /// A way to define an alternative message.
        /// </summary>
        public Func<TMsgData, string> AltMsg { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Construct stuff.
        /// </summary>
        protected MsgBase(MsgLevels level, int code)
        {
            Level = level;
            Code = code;
        }

        #endregion

        #region Methods

        /// <summary>
        /// By default we want to return the ToString() from the data.
        /// </summary>
        /// <returns></returns>
        protected virtual string BuildMsg()
        {
            return ToString();
        }

        #endregion

    }
}