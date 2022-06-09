using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Employee
{
    public class EmployeeAccrual : Entity<EmployeeAccrual>, IHasModifiedOptionalData
    {
        public virtual int EmployeeAccrualId { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual int ClientAccrualId { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual bool IsAllowScheduledAwards { get; set; }
        public virtual DateTime? WaitingPeriodDate { get; set; }
        public virtual bool IsActive { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual ClientAccrual ClientAccrual { get; set; }
    }
}
