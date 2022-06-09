using System;

namespace Dominion.Core.Dto.Payroll
{
    public class CurrentNormalPayrollDto
    {
        public int? PayrollId { get; set; }
        public DateTime? CheckDate { get; set; }
        public bool? IsOpen { get; set; }
        public DateTime? PeriodEnded { get; set; }
        public DateTime? PeriodStart { get; set; }
        public string EmployeePayCheckStubMessage { get; set; }
    }
}