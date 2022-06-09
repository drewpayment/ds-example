using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Entities;
using Dominion.LaborManagement.Dto.Enums;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantJobSite : Entity<ApplicantJobSite>
    {
        public virtual ApplicantJobSiteEnum ApplicantJobSiteId { get; set; }
        public virtual string JobSiteDescription { get; set; }
        public virtual string JobSiteBaseUrl { get; set; }
        
    }
}