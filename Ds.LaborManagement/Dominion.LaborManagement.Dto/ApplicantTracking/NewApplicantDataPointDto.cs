using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class NewApplicantDataPointDto
    {
        public DateTime Date { get; set; }
        public int Applicants { get; set; }
        public IDictionary<string,int> ExternalApplicants { get; set; }
    }
}
