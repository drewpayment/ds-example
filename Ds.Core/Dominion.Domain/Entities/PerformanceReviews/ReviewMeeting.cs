using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public partial class ReviewMeeting : Entity<ReviewMeeting>
    {
        public int       MeetingId     { get; set; } 
        public int       ReviewId      { get; set; } 
        public DateTime? CompletedDate { get; set; } 

        //FOREIGN KEYS
        public virtual Meeting Meeting { get; set; } 
        public virtual Review Review { get; set; } 
    }
}
