using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Constants;
using Dominion.Utility.Msg;
using Dominion.Utility.Msg.Identifiers;
using Dominion.Utility.Msg.Specific;

namespace Dominion.Utility.Security.Msg
{
    public class NotYourResourceMsg : MsgBase<NotYourResourceMsg>
    {
        /// <summary>
        /// Constructor.
        /// By default this is a fatal error.
        /// </summary>
        /// <param name="ex">Exception object.</param>
        public NotYourResourceMsg()
            : base(MsgLevels.Fatal, MsgCodes.ActionNotAllowed)
        {
        }

        /// <summary>
        /// Method that builds the default message.
        /// </summary>
        /// <returns></returns>
        protected override string BuildMsg()
        {
            return MessageConstants.RESOUCE_AUTHORIZATION_ERROR;
        }

    }
}
