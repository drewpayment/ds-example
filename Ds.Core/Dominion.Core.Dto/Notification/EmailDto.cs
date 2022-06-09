using Dominion.Core.Dto.Client;
namespace Dominion.Core.Dto.Notification
{
    public class EmailDto
    {
        public string ToAddress  { get; set; }
        public string Subject    { get; set; }
        public string Body       { get; set; }
        public bool   IsBodyHtml { get; set; }
        public ClientSMTPSettingDto ClientSMTPSetting { get; set; }
    }
}
