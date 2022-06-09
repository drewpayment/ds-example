using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Employee;

namespace Dominion.Core.Dto.Payroll
{
    public class OpenPayrollDetailDto
    {
        public int                    PayrollId                       { get; set; }
        public DateTime               PeriodStart                     { get; set; }
        public DateTime               PeriodEnded                     { get; set; }
        public DateTime               CheckDate                       { get; set; }
        public bool                   IsOpen                          { get; set; }
        public PayrollRunType         PayrollRunId                    { get; set; }
        public string                 PayrollRunDescription           { get; set; }
        public bool?                  IsFrequencyWeekly               { get; set; }
        public bool?                  IsFrequencyBiWeekly             { get; set; }
        public bool?                  IsFrequencyAltBiWeekly          { get; set; }
        public bool?                  IsFrequencySemiMonthly          { get; set; }
        public bool?                  IsFrequencyMonthly              { get; set; }
        public bool?                  IsFrequencyQuarterly            { get; set; }
        public DateTime?              FrequencyBiWeeklyPeriodStart    { get; set; }
        public DateTime?              FrequencyBiWeeklyPeriodEnded    { get; set; }
        public DateTime?              FrequencyAltBiWeeklyPeriodStart { get; set; }
        public DateTime?              FrequencyAltBiWeeklyPeriodEnded { get; set; }
        public DateTime?              FrequencySemiMonthlyPeriodStart { get; set; }
        public DateTime?              FrequencySemiMonthlyPeriodEnded { get; set; }
        public DateTime?              FrequencyMonthlyPeriodStart     { get; set; }
        public DateTime?              FrequencyMonthlyPeriodEnded     { get; set; }
        public DateTime?              FrequencyQuarterlyPeriodStart   { get; set; }
        public DateTime?              FrequencyQuarterlyPeriodEnded   { get; set; }
        public int                    EmployeesPaid                   { get; set; }
        public int                    NewHires                        { get; set; }
        public int                    EarningsOutOfBalance            { get; set; }
        public List<BasicEmployeeDto> NewHireEmployees                { get; set; }

        public OpenPayrollDetailDto()
        {
            NewHireEmployees = new List<BasicEmployeeDto>();
        }

    }
}
