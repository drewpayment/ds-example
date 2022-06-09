namespace Dominion.Core.Dto.Notification
{
    public class NotificationContactPreferences : IRecipient
    {
        public int?   UserId      { get; set; }
        public int?   EmployeeId  { get; set; }
        public int?   ApplicantId { get; set; }
        public string Email       { get; set; }
        public string PhoneNumber { get; set; }
        public bool   SendEmail   { get; set; }
        public bool   SendSms     { get; set; }
    }
}
