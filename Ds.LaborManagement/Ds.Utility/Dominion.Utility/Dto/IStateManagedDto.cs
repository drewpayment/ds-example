namespace Dominion.Utility.Dto
{
    /// <summary>
    /// Represents a DTO that uses state tracking to indicate object changes.
    /// </summary>
    public interface IStateManagedDto
    {
        /// <summary>
        /// State of the DTO. Used for change tracking.
        /// </summary>
        DtoState DtoState { get; set; }
    }
}
