using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Labor
{
    public class JobProfileResponsibilities : Entity<JobProfileResponsibilities>
    {
        public virtual int JobProfileId { get; set; }
        public virtual int JobResponibilityId { get; set; }

        public virtual JobProfile JobProfile { get; set; }
        public virtual JobResponsibilities JobResponsibilities { get; set; }
    }
}
