using Dominion.Core.Dto.Contact;

namespace Dominion.Core.Dto.EEOC
{
    public class EEOCOrganizationDto
    {
        public int EEOCOrganizationID { get; set; }
        public int FEIN { get; set; }
        public string CompanyNumber { get; set; }
        public int NAICSCode { get; set; }
        public string NORCUserID { get; set; }
        public byte? QuestionB2C { get; set; }
        public byte? QuestionC1 { get; set; }
        public byte? QuestionC2 { get; set; }
        public byte? QuestionC3 { get; set; }
        public string DunAndBradstreetNumber { get; set; }
        public int? CertifyingOfficialId { get; set; }
        public PersonDto CertifyingOfficial { get; set; }
    }
}