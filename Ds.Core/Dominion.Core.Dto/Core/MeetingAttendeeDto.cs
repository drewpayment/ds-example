using Dominion.Core.Dto.Contact.Search;

namespace Dominion.Core.Dto.Core
{
    public class MeetingAttendeeDto : ContactSearchDto, IMeetingAttendee
    {
        public int    MeetingAttendeeId { get; set; }
        public int    MeetingId         { get; set; }
    }
}