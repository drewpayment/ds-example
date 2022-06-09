using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Labor
{
    public class JobResponsibilities : Entity<JobResponsibilities>, IHasModifiedData
    {
        public virtual int JobResponsibilityId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }

        public virtual Client Client { get; set; }

        public virtual ICollection<JobProfileResponsibilities> JobProfileResponsibilities { get; set; }
    }
}