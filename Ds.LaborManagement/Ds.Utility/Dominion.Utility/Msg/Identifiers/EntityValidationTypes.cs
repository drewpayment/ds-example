namespace Dominion.Utility.Msg.Identifiers
{
    /// <summary>
    /// Different categories for entity validation.
    /// </summary>
    public enum EntityValidationTypes
    {
        General, 
        Required, 
        OutOfRange, 
        Incompatible, 
        ActionNotAllowed, 
        InvalidFormat, 
        NotFound
    }
}