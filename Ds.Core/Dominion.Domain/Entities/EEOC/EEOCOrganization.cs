using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Contact;

namespace Dominion.Domain.Entities.EEOC
{
    public class EEOCOrganization : Entity<EEOCOrganization>
    {
        public virtual int EEOCOrganizationID { get; set; }
        public virtual int FEIN { get; set; }
        public virtual string CompanyNumber { get; set; }
        public virtual int NAICSCode { get; set; }
        public virtual string NORCUserID { get; set; }
        public virtual byte? QuestionB2C { get; set; }
        public virtual byte? QuestionC1 { get; set; }
        public virtual byte? QuestionC2 { get; set; }
        public virtual byte? QuestionC3 { get; set; }
        public virtual string DunAndBradstreetNumber { get; set; }
        public virtual int? CertifyingOfficialId { get; set; }

        public virtual Person CertifyingOfficial { get; set; }
        
    }
}

