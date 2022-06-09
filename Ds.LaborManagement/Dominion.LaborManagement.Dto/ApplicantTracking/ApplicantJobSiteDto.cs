using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.LaborManagement.Dto.Enums;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantJobSiteDto
    {
        public ApplicantJobSiteEnum ApplicantJobSiteId { get; set; }
        public string JobSiteDescription { get; set; }
        public string JobSiteBaseUrl { get; set; }
    }
}
