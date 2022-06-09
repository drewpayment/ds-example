using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    [Serializable]
    public partial class EmailDto
    {
        public string EmailFrom { get; set; }
        public string EmailFromName { get; set; }
        public string EmailTo { get; set; }
        public string EmailToName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public bool? IsBodyHtml { get; set; }
    }
}