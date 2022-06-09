using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.Tax;

namespace Dominion.Domain.Entities.Payroll
{
    public class ClientEarning : Entity<ClientEarning>
    {
        public virtual int      ClientEarningId          { get; set; } //pk
        public virtual int      ClientId                 { get; set; } //fk
        public virtual string   Description              { get; set; }
        public virtual string   Code                     { get; set; }
        public virtual double   Percent                  { get; set; }
        public virtual bool     IsShowOnStub             { get; set; }
        public virtual bool     IsShowYtdHours           { get; set; }
        public virtual bool     IsShowYtdDollars         { get; set; }
        public virtual byte     CalcShiftPremium         { get; set; }
        public virtual bool     IsTips                   { get; set; }
        public virtual bool     IsDefault                { get; set; }
        public virtual ClientEarningCategory EarningCategoryId { get; set; } //fk
        public virtual bool     IsIncludeInDeductions    { get; set; }
        public virtual bool     IsEic                    { get; set; }
        public virtual DateTime Modified                 { get; set; }
        public virtual string   ModifiedBy               { get; set; }
        public virtual decimal? AdditionalAmount         { get; set; }
        public virtual bool     IsIncludeInOvertimeCalcs { get; set; }
        public virtual bool     IsActive                 { get; set; }
        public virtual bool     IsBlockFromTimeClock     { get; set; }
        public virtual bool     IsIncludeInAvgRate       { get; set; }
        public virtual bool     IsShowOnlyIfCurrent      { get; set; }
        public virtual int      BlockedSecurityUser      { get; set; }
        public virtual bool     IsUpMinWage              { get; set; }
        public virtual bool     IsShowLifetimeHours      { get; set; }
        public virtual bool     IsExcludeHrsInArpCalc    { get; set; }
        public virtual bool     IsExcludePayInArpCalc    { get; set; }
        public virtual bool     IsServiceChargeTips      { get; set; }
        public virtual bool     IsAcaWorkedHours         { get; set; }
        public virtual int?     TaxOptionId              { get; set; } // fk
        public virtual bool     IsReimburseTaxes         { get; set; }
        public virtual bool     IsBasePay                { get; set; }
        public virtual int      EmergencyLeave           { get; set; }

        // REVERSE NAVIGATION
        public virtual ICollection<PayrollPayDataDetail> PayrollPayDataDetail { get; set; } // many-to-one;
        public virtual ICollection<PayrollControlTotal>  PayrollControlTotal { get; set; } // many-to-one;

        /// <summary>
        /// Represents the tax-types that should never be withheld from employee pay for the current earning.
        /// </summary>
        public virtual ICollection<ClientEarningWithholdingOverride> WithholdingOverrides { get; set; }

        // FOREIGN KEYS
        public virtual Client Client { get; set; }
        public virtual EarningCategory EarningCategory { get; set; }
        public virtual TaxOption TaxOption { get; set; }
        public virtual ICollection<OneTimeEarning> OneTimeEarning { get; set; }
    }
}