using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Payroll
{
    public class Payroll : Entity<Payroll>, IHasModifiedUserNameData
    {
        public virtual int PayrollId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual DateTime PeriodStart { get; set; }
        public virtual DateTime PeriodEnded { get; set; }
        public virtual DateTime CheckDate { get; set; }
        public virtual int PayrollSpecialOptionId { get; set; }
        public virtual int PayrollLastPayrollId { get; set; }
        public virtual PayrollRunType PayrollRunId { get; set; }
        public virtual string PayrollControl { get; set; }
        public virtual bool? IsFrequencyWeekly { get; set; }
        public virtual bool? IsFrequencyBiWeekly { get; set; }
        public virtual bool? IsFrequencyAltBiWeekly { get; set; }
        public virtual bool? IsFrequencySemiMonthly { get; set; }
        public virtual bool? IsFrequencyMonthly { get; set; }
        public virtual bool? IsFrequencyQuarterly { get; set; }
        public virtual byte CalendarYear { get; set; }
        public virtual byte QuarterToDate { get; set; }
        public virtual byte PeriodToDate { get; set; }
        public virtual byte FiscalYear { get; set; }
        public virtual byte DeductionAccrual { get; set; }
        public virtual byte DeductionBalanceLimit { get; set; }
        public virtual byte AccruedHours { get; set; }
        public virtual bool IsOpen { get; set; }
        public virtual bool? IsFirstOfMonth { get; set; }
        public virtual bool? IsFirstOfQuarter { get; set; }
        public virtual bool? IsFirstOfYear { get; set; }
        public virtual bool? IsFirstOfFiscal { get; set; }
        public virtual bool? IsLastOfMonth { get; set; }
        public virtual bool? IsLastOfQuarter { get; set; }
        public virtual bool? IsLastOfYear { get; set; }
        public virtual bool? IsLastOfFiscal { get; set; }
        public virtual string EmployeePaycheckStubMessage { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual bool? IsIncludeDirectDeposits { get; set; }
        public virtual bool? IsIncludeBonds { get; set; }
        public virtual PayrollDirectDepositOptionType? DirectDepositsOnMultipleChecks { get; set; }
        public virtual int? RunId { get; set; }
        public virtual double? GlAccrualPercentage { get; set; }
        public virtual DateTime? FrequencyBiWeeklyPeriodStart { get; set; }
        public virtual DateTime? FrequencyBiWeeklyPeriodEnded { get; set; }
        public virtual DateTime? FrequencyAltBiWeeklyPeriodStart { get; set; }
        public virtual DateTime? FrequencyAltBiWeeklyPeriodEnded { get; set; }
        public virtual DateTime? FrequencySemiMonthlyPeriodStart { get; set; }
        public virtual DateTime? FrequencySemiMonthlyPeriodEnded { get; set; }
        public virtual DateTime? FrequencyMonthlyPeriodStart { get; set; }
        public virtual DateTime? FrequencyMonthlyPeriodEnded { get; set; }
        public virtual DateTime? FrequencyQuarterlyPeriodStart { get; set; }
        public virtual DateTime? FrequencyQuarterlyPeriodEnded { get; set; }
        public virtual double? SupFedRate { get; set; }
        public virtual short? SupFedOption { get; set; }
        public virtual PayrollDirectDepositOptionType DirectDepositOnMainChecks { get; set; }
        public virtual bool IsPayrollConfirmed { get; set; }
        public virtual bool IsHolidayCheckDate { get; set; }

        // REVERSE NAVIGATION
        public virtual ICollection<PayrollPayData> PayData { get; set; } // many-to-one;
        public virtual ICollection<PayrollHistory> PayrollHistory { get; set; }
        public virtual ICollection<OneTimeEarningPayroll> OneTimeEarningPayrolls { get; set; }
        public virtual PayrollRun PayrollRun { get; set; }
        public virtual Client Client { get; set; }
        public virtual ICollection<ClockClientLockUser> ClockClientLockUsers { get; set; }
    }
}