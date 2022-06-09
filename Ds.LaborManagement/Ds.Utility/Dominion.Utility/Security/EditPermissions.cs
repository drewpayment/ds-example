namespace Dominion.Utility.Security
{
    /// <summary>
    /// The type of edits.
    /// Edit is considered add or modify.
    /// </summary>
    public enum EditPermissions
    {
        /// <summary>
        /// Permission hasn't be defined in current case.
        /// </summary>
        NotDefined, 

        /// <summary>
        /// User cannot edit.
        /// </summary>
        ViewOnly, 

        /// <summary>
        /// User can directly edit.
        /// A direct edit skips any request workflow.
        /// </summary>
        DirectChange, 

        /// <summary>
        /// User has to request an edit.
        /// </summary>
        RequestChange
    }
}