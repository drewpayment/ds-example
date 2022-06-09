using System;


namespace Dominion.Core.Dto.Client
{
    [Serializable]
    public class ClientSMTPSettingDto
    {
        public virtual int ClientSMTPSettingId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string SMTPHost { get; set; }
        public virtual string SMTPPort { get; set; }
        public virtual string SenderEmail { get; set; }
        public virtual string SMTPLogin { get; set; }
        public virtual string SMTPPassword { get; set; }
        public virtual string SecureConnection { get; set; }
    }
}
