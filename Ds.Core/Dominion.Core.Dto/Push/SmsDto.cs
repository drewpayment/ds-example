using System;

namespace Dominion.Core.Dto.Push
{
    public class SmsDto
    {
        public int       Id        { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime   { get; set; }
        public string    Content   { get; set; }
    }
}
