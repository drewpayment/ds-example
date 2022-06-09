using Dominion.Authentication.Dto;

namespace Dominion.Core.Dto.Notification
{
    public class SmsDto
    {
        public int           UserId        { get; set; }
        public string        PhoneNumber   { get; set; }
        public string        Message       { get; set; }
    }
}
