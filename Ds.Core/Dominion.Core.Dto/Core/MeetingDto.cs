using System;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Core
{
    public class MeetingDto : IMeeting
    {
        public int       MeetingId     { get; set; }
        public string    Title         { get; set; }
        public string    Description   { get; set; }
        public string    Location      { get; set; }
        public DateTime  StartDateTime { get; set; }
        public DateTime? EndDateTime   { get; set; }

        public IEnumerable<MeetingAttendeeDto> Attendees { get; set; }

        #region IMeeting Methods

        IEnumerable<IMeetingAttendee> IMeeting.GetAttendees()
        {
            return Attendees;
        }

        #endregion
    }
}