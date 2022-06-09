using System.Text;
using Dominion.Utility.Msg.Identifiers;

namespace Dominion.Utility.Security.Msg
{
    /// <summary>
    /// Message used when an action is not allowed to be performed.
    /// </summary>
    public class ActionNotAllowedMsg : SecurityMsg<ActionNotAllowedMsg>
    {
        public const string THE_FOLLOWING_ROLE_IS_REQUIRED = "The following role is required:";

        /// <summary>
        /// The action that was not allowed.
        /// </summary>
        public IActionType ActionNotAllowed { get; private set; }

        /// <summary>
        /// Instantiates a new ActionNotAllowedMsg object.
        /// </summary>
        /// <param name="actionNotAllowed">The action that is not allowed to be performed.</param>
        public ActionNotAllowedMsg(IActionType actionNotAllowed)
            : base(MsgLevels.Error, SecurityMessageType.ActionNotAllowed)
        {
            ActionNotAllowed = actionNotAllowed;
        }

        /// <summary>
        /// Constructs the message text.
        /// </summary>
        /// <returns>Message text.</returns>
        protected override string BuildMsg()
        {
            return "The user does not have permission to perform '" + ActionNotAllowed.Designation + "' actions.";
        }

                /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetActionPermissionErrorMessage()
        {
            var msg = string.Format(
                "The user does not have permission to perform actions of type '{0}' ({1}).", 
                ActionNotAllowed.Designation, 
                ActionNotAllowed.Label);

            if (ActionNotAllowed is ActionType)
            {
                var rolesMsg = new StringBuilder();
                var actionType = ActionNotAllowed as ActionType;
                if (actionType.Roles.Count > 0)
                {
                    if (actionType.Roles.Count == 1)
                        rolesMsg.Append(
                            Constants.CommonConstants.SINGLE_SPACE +
                            THE_FOLLOWING_ROLE_IS_REQUIRED +
                            Constants.CommonConstants.SINGLE_SPACE);
                    else
                        rolesMsg.Append(" One of the following roles are required: ");

                    rolesMsg.Append(actionType.Roles[0]);

                    for (int i = 1; i < actionType.Roles.Count; i++)
                        rolesMsg.Append(", " + actionType.Roles[i]);

                    rolesMsg.Append(".");
                }

                msg += rolesMsg;
            }

            return msg;
        }
    }
}