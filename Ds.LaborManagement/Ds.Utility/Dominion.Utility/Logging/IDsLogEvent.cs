namespace Dominion.Utility.Logging
{
    public interface IDsLogEvent
    {
        string CallStack { get; set; }
        string Detail { get; set; }
        string Title { get; set; }
        string UserName { get; set; }
    }
}