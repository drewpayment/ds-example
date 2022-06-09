using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Payroll;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeePayBasicDto
    {
        public int              EmployeePayId    { get; set; }
        public int              EmployeeId       { get; set; }
        public PayFrequencyType PayFrequencyId   { get; set; }
        public string           PayFrequencyDesc { get; set; }
        public PayType          PayType          { get; set; }
        public double?          SalaryAmount     { get; set; }
        public int              AnnualPayPeriods { get; set; }
    }
}
