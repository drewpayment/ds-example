namespace Dominion.Core.Dto.Performance
{
    public interface ISingleFeedbackResponse : IHasFeedbackResponseData
    {
        int              ResponseItemId    { get; set; }
    }
}