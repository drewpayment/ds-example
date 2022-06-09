namespace Dominion.Core.Dto.Performance
{
    public class FeedbackItemDto
    {
        public int    FeedbackId     { get; set; }
        public int    FeedbackItemId { get; set; }
        public string ItemText       { get; set; }
        public bool   Checked        { get; set; }
    }
}