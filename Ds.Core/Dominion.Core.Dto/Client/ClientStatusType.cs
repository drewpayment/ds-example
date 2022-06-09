namespace Dominion.Core.Dto.Client
{
    /// <summary>
    /// Enum representation of the dbo.ClientStatus table.
    /// </summary>
    public enum ClientStatusType
    {
        Active               = 1,
        Seasonal             = 2,
        InfrequentProcessing = 3,
        Terminated           = 4,
        Parallel             = 5,
        TerminatedWithAccess = 6
    }
}
