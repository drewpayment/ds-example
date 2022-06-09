namespace Dominion.Utility.Logging
{
    public interface IDsLogIdentity
    {
        string UserName { get; set; }
        int? CLientId { get; set; }
    }
}