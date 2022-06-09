using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Core
{
    public partial class MeetingAttendee : Entity<MeetingAttendee>
    {
        public int  MeetingAttendeeId { get; set; } 
        public int  MeetingId         { get; set; } 
        public int? UserId            { get; set; } 
        public int? EmployeeId        { get; set; }

        public virtual Meeting Meeting { get; set; }
        public virtual User.User User { get; set; }
        public virtual Employee.Employee Employee { get; set; }
    }
}