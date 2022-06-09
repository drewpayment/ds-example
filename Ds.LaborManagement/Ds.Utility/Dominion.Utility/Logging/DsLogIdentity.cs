namespace Dominion.Utility.Logging
{
    public class DsLogIdentity : IDsLogIdentity
    {
        public string UserName { get; set; }
        public int? CLientId { get; set; }
    }
}