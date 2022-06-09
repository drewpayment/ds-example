using System;

namespace Dominion.Core.Dto.Performance
{
    public class FeedbackResponseItemData
    {
        public int       ResponseItemId { get; set; }
        public int       ResponseId     { get; set; }
        public int?      FeedbackItemId { get; set; }
        public bool?     BooleanValue   { get; set; }
        public string    TextValue      { get; set; }
        public DateTime? DateValue      { get; set; }
        public FeedbackItemDto FeedbackItem { get; set; }
    }
}