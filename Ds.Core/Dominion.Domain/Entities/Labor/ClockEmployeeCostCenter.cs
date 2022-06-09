using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public partial class ClockEmployeeCostCenter : Entity<ClockEmployeeCostCenter>, IHasModifiedData
    {
        public virtual int      ClockEmployeeCostCenterId   { get; set; } 
        public virtual int      EmployeeId                  { get; set; } 
        public virtual int      ClientCostCenterId          { get; set; } 
        public virtual int      ModifiedBy                  { get; set; } 
        public virtual DateTime Modified                    { get; set; } 

        public virtual Employee.Employee Employee { get; set; } 
        public virtual ClockEmployee ClockEmployee { get; set; }
        public virtual ClientCostCenter CostCenter { get; set; } 
    }
}
