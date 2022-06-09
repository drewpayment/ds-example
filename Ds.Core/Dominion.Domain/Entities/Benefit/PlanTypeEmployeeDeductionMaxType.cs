using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Benefit
{
    public class PlanTypeEmployeeDeductionMaxType : Entity<PlanTypeEmployeeDeductionMaxType>
    {
        public int PlanTypeId { get; set; }
        public int EmployeeDeductionMaxTypeId { get; set; }
    }
}
