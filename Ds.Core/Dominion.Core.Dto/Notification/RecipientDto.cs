using Dominion.Core.Dto.User;

namespace Dominion.Core.Dto.Notification
{
    public class RecipientDto
    {
        public int?     UserId       { get; set; }
        public UserType UserType     { get; set; }
        public string   FirstName    { get; set; }
        public string   LastName     { get; set; }
        public string   EmailAddress { get; set; }
        public string   PhoneNumber  { get; set; }
        public bool     SendEmail    { get; set; }
        public bool     SendSms      { get; set; }
    }
}
