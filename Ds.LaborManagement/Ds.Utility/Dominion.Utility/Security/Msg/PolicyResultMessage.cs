using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Msg.Identifiers;

namespace Dominion.Utility.Security.Msg
{
    public class PolicyResultMessage : SecurityMsg<PolicyResultMessage>
    {
        private readonly string _message;

        public PolicyResultMessage(string message) : base(MsgLevels.Fatal, SecurityMessageType.ActionNotAllowed)
        {
            _message = message;
        }

        public override string ToString()
        {
            return _message;
        }
    }
}
