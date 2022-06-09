using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Payroll;

namespace Dominion.Core.Dto.Employee
{
    public partial class EmployeeDeductionAmountTypeInfoDto
    {
        public EmployeeDeductionAmountType EmployeeDeductionAmountTypeId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public byte? DisplayOrder { get; set; }
    }
}
