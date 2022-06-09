using System;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public partial class FeedbackResponseItem : Entity<FeedbackResponseItem>
    {
        public int       ResponseItemId { get; set; } 
        public int       ResponseId     { get; set; } 
        public bool?     BooleanValue   { get; set; } 
        public DateTime? DateValue      { get; set; } 
        public string    TextValue      { get; set; } 
        public int?      FeedbackItemId { get; set; } 

        //FOREIGN KEYS
        public virtual FeedbackResponse FeedbackResponse { get; set; } 
        public virtual FeedbackItem     FeedbackItem     { get; set; } 
    }
}