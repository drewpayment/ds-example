namespace Dominion.Core.Dto.Performance
{
    public class FeedbackSetupDto : FeedbackBasicDto
    {
        public int?  ClientId             { get; set; }
        public bool IsSupervisor         { get; set; }
        public bool IsPeer               { get; set; }
        public bool IsSelf               { get; set; }
        public bool IsActionPlan         { get; set; }
        public bool IsVisibleToEmployee { get; set; }
    }
}