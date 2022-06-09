using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Contact;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.EEOC
{
    public class ClientEEOC : Entity<ClientEEOC>,IHasModifiedData
    {
        public virtual int      ClientId { get; set; } 
        public virtual string   CompanyNumber { get; set; } 
        public virtual string   DunBradstreetNumber { get; set; } 
        public virtual string   NaicsCode { get; set; } 
        public virtual string   NorcUserId { get; set; }
        public virtual int?      CertifyingOfficialId { get; set; } 
        public virtual byte     QuestionB2C { get; set; } 
        public virtual byte     QuestionC1 { get; set; } 
        public virtual byte     QuestionC2 { get; set; } 
        public virtual byte     QuestionC3 { get; set; } 
        public virtual DateTime Modified { get; set; } 
        public virtual int      ModifiedBy { get; set; } 

        //FOREIGN KEYS
        public virtual Client Client { get; set; } 
        public virtual Person CertfyingOfficial { get; set; } 
    }
}
