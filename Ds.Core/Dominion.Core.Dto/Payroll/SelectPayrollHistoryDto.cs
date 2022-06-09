using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class SelectPayrollHistoryDto   
    {
        public int       PayrollId             { get; set; }
        public int       PayrollHistoryId      { get; set; }
        public DateTime  CheckDate             { get; set; }
        public DateTime? PeriodEnded           { get; set; }
        public DateTime? PeriodStart           { get; set; }
        public DateTime  RunDate               { get; set; }
        public decimal   TotalGross            { get; set; }
        public decimal   TotalNet              { get; set; }
        public bool      IsOpen                { get; set; }
        public string    PayrollRunDescription { get; set; }
        public string    Year                  { get; set; }
        public string    RunBy                 { get; set; }
        public PayrollRunType? PayrollRunId    { get; set; }
        public virtual bool? IsFrequencyWeekly { get; set; }
        public virtual bool? IsFrequencyBiWeekly { get; set; }
        public virtual bool? IsFrequencyAltBiWeekly { get; set; }
        public virtual bool? IsFrequencySemiMonthly { get; set; }
        public virtual bool? IsFrequencyMonthly { get; set; }
        public virtual bool? IsFrequencyQuarterly { get; set; }
        public virtual DateTime? FrequencyBiWeeklyPeriodStart    { get; set; }
        public virtual DateTime? FrequencyBiWeeklyPeriodEnded    { get; set; }
        public virtual DateTime? FrequencyAltBiWeeklyPeriodStart { get; set; }
        public virtual DateTime? FrequencyAltBiWeeklyPeriodEnded { get; set; }
        public virtual DateTime? FrequencySemiMonthlyPeriodStart { get; set; }
        public virtual DateTime? FrequencySemiMonthlyPeriodEnded { get; set; }
        public virtual DateTime? FrequencyMonthlyPeriodStart     { get; set; }
        public virtual DateTime? FrequencyMonthlyPeriodEnded     { get; set; }
        public virtual DateTime? FrequencyQuarterlyPeriodStart   { get; set; }
        public virtual DateTime? FrequencyQuarterlyPeriodEnded   { get; set; }
    }
}
