using System;

namespace Dominion.Core.Dto.Tax.EmployeeTaxAdmin
{
    public class EmployeeTaxCostCenterDto
    {
        public int EmployeeTaxId { get; set; }
        public int ClientCostCenterId { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
    }
}
