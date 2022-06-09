using System;

using Dominion.Utility.Msg.Identifiers;

namespace Dominion.Utility.Msg.Specific
{
    public class NullReferenceMsg<TParam> : DataNotFoundMsg<TParam>
    {
        public string ObjectName { get; private set; }
        public string StackTrace { get; private set; }

        public NullReferenceMsg(string objectName, MsgLevels level = MsgLevels.Error)
            : base(level)
        {
            this.ObjectName = objectName;
            StackTrace = Environment.StackTrace;
        }

        protected override string BuildMsg()
        {
            return (this.ObjectName != null ? "The " + this.ObjectName : "Value") + " was not specified.";
        }
    }
}
