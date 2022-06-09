using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.LaborManagement.Dto.Enums;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ClientJobSite : Entity<ClientJobSite>, IHasModifiedData
    {
        public virtual int ClientJobSiteId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual ApplicantJobSiteEnum ApplicantJobSiteId { get; set; }
        public virtual string Code { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual int Counter { get; set; }
        public virtual bool? SharePosts { get; set; }
        public virtual string Email { get; set; }

        public virtual ApplicantJobSite ApplicantJobSite { get; set; }
    }
}
