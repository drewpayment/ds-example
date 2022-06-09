namespace Dominion.Core.Dto.Common
{
    /// <summary>
    /// Defines if record has been approved for actual use.
    /// The term 'Insert' refers to the idea that it's allowed to be considered a usable record  from the database.
    /// </summary>
    public enum InsertStatus : byte
    {
        /// <summary>
        /// This record should not be considered a usable record.
        /// </summary>
        Pending = 0, 

        /// <summary>
        /// This is a usable record.
        /// </summary>
        Approved = 1
    }
}