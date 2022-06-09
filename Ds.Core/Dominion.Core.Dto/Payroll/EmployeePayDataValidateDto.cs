using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class EmployeePayDataValidateDto
    {
        public int EmployeeId { get; set; }
        public int? ClientRateId { get; set; }
        public double? Hours { get; set; }
        public double? Pay { get; set; }
        public ClientEarningDto ClientEarningInfo { get; set; }
    }
}
