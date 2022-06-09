using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Employee;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientSMTPSetting : Entity<ClientSMTPSetting>
    {
        public virtual int ClientSMTPSettingId { get; set; } 
        public virtual int ClientId { get; set; } 
        public virtual string SMTPHost { get; set; } 
        public virtual string SMTPPort { get; set; }
        public virtual string SenderEmail { get; set; }
        public virtual string SMTPLogin { get; set; }
        public virtual string SMTPPassword { get; set; }
        public virtual string SecureConnection { get; set; }
        public virtual DateTime? Modified { get; set; } 
        public virtual int ModifiedBy { get; set; } 
        
        
        public virtual Client Client { get; set; }
    }
}
