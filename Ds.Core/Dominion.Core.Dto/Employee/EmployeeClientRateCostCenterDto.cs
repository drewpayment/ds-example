using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeeClientRateCostCenterDto
    {
        public int EmployeeClientRateCostCenterId { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public int ClientRateId { get; set; }
        public int ClientCostCenterId { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
    }
}