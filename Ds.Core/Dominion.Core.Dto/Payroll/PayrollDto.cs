using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Client;

namespace Dominion.Core.Dto.Payroll
{
    public class PayrollDto
    {
        public int PayrollId { get; set; }
        public int ClientId { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnded { get; set; }
        public DateTime CheckDate { get; set; }
        public int PayrollSpecialOptionId { get; set; }
        public int PayrollLastPayrollId { get; set; }
        public PayrollRunType PayrollRunId { get; set; }
        public string PayrollControl { get; set; }
        public bool? IsFrequencyWeekly { get; set; }
        public bool? IsFrequencyBiWeekly { get; set; }
        public bool? IsFrequencyAltBiWeekly { get; set; }
        public bool? IsFrequencySemiMonthly { get; set; }
        public bool? IsFrequencyMonthly { get; set; }
        public bool? IsFrequencyQuarterly { get; set; }
        public byte CalendarYear { get; set; }
        public byte QuarterToDate { get; set; }
        public byte PeriodToDate { get; set; }
        public byte FiscalYear { get; set; }
        public byte DeductionAccrual { get; set; }
        public byte DeductionBalanceLimit { get; set; }
        public byte AccruedHours { get; set; }
        public bool IsOpen { get; set; }
        public bool? IsFirstOfMonth { get; set; }
        public bool? IsFirstOfQuarter { get; set; }
        public bool? IsFirstOfYear { get; set; }
        public bool? IsFirstOfFiscal { get; set; }
        public bool? IsLastOfMonth { get; set; }
        public bool? IsLastOfQuarter { get; set; }
        public bool? IsLastOfYear { get; set; }
        public bool? IsLastOfFiscal { get; set; }
        public string EmployeePaycheckStubMessage { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsIncludeDirectDeposits { get; set; }
        public bool? IsIncludeBonds { get; set; }
        public PayrollDirectDepositOptionType? DirectDepositsOnMultipleChecks { get; set; }
        public int? RunId { get; set; }
        public double? GlAccrualPercentage { get; set; }
        public DateTime? FrequencyBiWeeklyPeriodStart { get; set; }
        public DateTime? FrequencyBiWeeklyPeriodEnded { get; set; }
        public DateTime? FrequencyAltBiWeeklyPeriodStart { get; set; }
        public DateTime? FrequencyAltBiWeeklyPeriodEnded { get; set; }
        public DateTime? FrequencySemiMonthlyPeriodStart { get; set; }
        public DateTime? FrequencySemiMonthlyPeriodEnded { get; set; }
        public DateTime? FrequencyMonthlyPeriodStart { get; set; }
        public DateTime? FrequencyMonthlyPeriodEnded { get; set; }
        public DateTime? FrequencyQuarterlyPeriodStart { get; set; }
        public DateTime? FrequencyQuarterlyPeriodEnded { get; set; }
        public double? SupFedRate { get; set; }
        public short? SupFedOption { get; set; }
        public PayrollDirectDepositOptionType DirectDepositOnMainChecks { get; set; }
        public bool IsPayrollConfirmed { get; set; }

        // REVERSE NAVIGATION
        //public ICollection<PayrollPayDataDto> PayData { get; set; } // many-to-one;
        public ICollection<PayrollHistoryDto> PayrollHistory { get; set; }

        public PayrollRunDto PayrollRun { get; set; }

        public ClientDto Client { get; set; }
    }
}
