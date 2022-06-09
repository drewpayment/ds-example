using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class GetNewApplicantsDateByDateSpanInnerJoinResultDto
    {
        public DateTime DateSubmitted { get; set; }
        public bool IsExternalApplicant { get; set; }
        public string JobSiteName { get; set; }
    }
}
