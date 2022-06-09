using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Labor;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientShiftSelected : Entity<ClientShiftSelected>
    {
        public int ClockClientShiftSelectedId { get; set; }
        public int ClockClientTimePolicyId { get; set; }
        public int ClientShiftId { get; set; }


        // RELATIONSHIPS

        public virtual ClockClientTimePolicy TimePolicy { get; set; }
        public virtual ClientShift Shift { get; set; }
    }
}
