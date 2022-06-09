namespace Dominion.Core.Dto.Performance
{
    public enum EvaluationStatus
    {
        SetupIncomplete = 0,
        ToDo            = 1,
        InProgress      = 2,
        PastDue         = 3,
        Submitted       = 4,
        NeedsApproval   = 5,
        Approved        = 6
    }
}