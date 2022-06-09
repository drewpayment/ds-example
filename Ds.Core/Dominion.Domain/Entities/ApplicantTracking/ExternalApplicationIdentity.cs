using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.LaborManagement.Dto.Enums;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public class ExternalApplicationIdentity : Entity<ExternalApplicationIdentity>
    {
        public int ApplicantApplicationHeaderId { get; set; }
        public ApplicantApplicationHeader ApplicantApplicationHeader { get; set; }
        public string ExternalApplicationId { get; set; }
        public ApplicantJobSiteEnum ApplicantJobSiteId { get; set; }
        public ApplicantJobSite ApplicantJobSite { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
