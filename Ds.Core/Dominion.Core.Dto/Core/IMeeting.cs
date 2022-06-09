using System;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Core
{
    public interface IMeeting
    {
        int       MeetingId     { get; set; }
        string    Title         { get; set; }
        string    Description   { get; set; }
        string    Location      { get; set; }
        DateTime  StartDateTime { get; set; }
        DateTime? EndDateTime   { get; set; }

        IEnumerable<IMeetingAttendee> GetAttendees();
    }
}