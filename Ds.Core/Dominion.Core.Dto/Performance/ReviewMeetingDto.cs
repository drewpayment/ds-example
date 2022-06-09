using System;
using Dominion.Core.Dto.Core;

namespace Dominion.Core.Dto.Performance
{
    public class ReviewMeetingDto : MeetingDto
    {
        public int       ReviewId       { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}