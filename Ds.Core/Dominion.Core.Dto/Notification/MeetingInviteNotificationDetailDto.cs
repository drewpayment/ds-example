using System;

namespace Dominion.Core.Dto.Notification
{
    public class MeetingInviteNotificationDetailDto
    {
        public int? ReviewId { get; set; }
        public int MeetingId { get; set; }
        public string Title { get; set; }
        public DateTime MeetingStart { get; set; }
        public string Location { get; set; }
        public string MeetingConductedBy { get; set; }
    }
}
