using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Contact;
using Dominion.Core.Dto.Location;

//Future Consideration: Consolidate with Person & Address
namespace Dominion.Core.Dto.EEOC
{
    [Serializable]
    public partial class ExportClientEEOCDto
    {
        public int    ClientId { get; set; }
        public string CompanyNumber { get; set; }
        public string DunBradstreetNumber { get; set; }
        public string NorcUserId { get; set; }
        public string NaicsCode { get; set; }
        public int?    CertifyingOfficialId { get; set; }
        public byte   QuestionB2C { get; set; }
        public byte   QuestionC1 { get; set; }
        public byte   QuestionC2 { get; set; }
        public byte   QuestionC3 { get; set; }

        /// <summary>
        /// This is the odd man out for questions.
        /// This is really a string that represents a start date and end date for the payroll period used in the report.
        /// Format: MMddyyyyMMddyy
        /// </summary>
        public string QuestionD1 { get; set; }


        //public  ClientDto Client { get; set; } 
        public PersonDto CertfyingOfficial { get; set; }
    }
}
