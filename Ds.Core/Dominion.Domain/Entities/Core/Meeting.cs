using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Core
{
    public partial class Meeting : Entity<Meeting>, IHasModifiedData
    {
        public int       MeetingId     { get; set; } 
        public string    Title         { get; set; } 
        public string    Description   { get; set; } 
        public string    Location      { get; set; } 
        public DateTime  StartDateTime { get; set; } 
        public DateTime? EndDateTime   { get; set; } 
        public DateTime  Modified      { get; set; } 
        public int       ModifiedBy    { get; set; }

        public virtual ICollection<MeetingAttendee> MeetingAttendees { get; set; }
        public virtual ReviewMeeting ReviewMeeting { get; set; } 
    }
}