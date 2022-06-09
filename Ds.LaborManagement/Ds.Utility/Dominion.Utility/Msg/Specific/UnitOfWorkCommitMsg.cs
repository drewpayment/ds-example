using Dominion.Utility.Msg.Identifiers;

namespace Dominion.Utility.Msg.Specific
{
    /// <summary>
    /// Any message data that needs to be recorded during a unit of work commital.
    /// </summary>
    public class UnitOfWorkCommitMsg : MsgList
    {
        /// <summary>
        /// A unique identifier to determine which UOW commit this message comes from.
        /// </summary>
        public string UniqueIdentifier { get; set; }

        /// <summary>
        /// Constructor.
        /// By default this is set as a fatal message.
        /// </summary>
        public UnitOfWorkCommitMsg(string uniqueIdentifier = Constants.CommonConstants.EMPTY_STRING)
            : base(MsgLevels.Fatal, MsgCodes.EntityFrameworkValidation)
        {
            UniqueIdentifier = uniqueIdentifier;
        }

    }
}