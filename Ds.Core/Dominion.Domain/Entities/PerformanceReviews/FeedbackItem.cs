using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public partial class FeedbackItem : Entity<FeedbackItem>, IHasModifiedData
    {
        public int      FeedbackItemId { get; set; } 
        public int      FeedbackId     { get; set; } 
        public string   ItemText       { get; set; } 
        public DateTime Modified       { get; set; } 
        public int      ModifiedBy     { get; set; } 

        //FOREIGN KEYS
        public virtual Feedback Feedback { get; set; } 
        public virtual ICollection<FeedbackResponseItem> FeedbackResponseItems { get; set; }
    }
}