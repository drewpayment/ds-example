using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Labor
{
    public partial class ClockClientAddHoursSelected : Entity<ClockClientAddHoursSelected>
    {
        public virtual int ClockClientTimePolicyId { get; set; }
        public virtual int ClockClientAddHoursId { get; set; }

        // ENTITY RELATIONSHIPS
        public virtual ClockClientTimePolicy TimePolicy { get; set; }
        public virtual ClockClientAddHours AddHours { get; set; }
    }
}
