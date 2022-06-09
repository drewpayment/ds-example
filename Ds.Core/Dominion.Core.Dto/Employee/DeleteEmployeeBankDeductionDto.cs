using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    public class DeleteEmployeeBankDeductionDto
    {
        public int EmployeeDeductionID { get; set; }
        public int EmployeeBankID { get; set; }
        public int ModifiedBy { get; set; }
    }
}
