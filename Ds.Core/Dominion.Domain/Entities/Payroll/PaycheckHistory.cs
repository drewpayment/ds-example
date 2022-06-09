using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Payroll
{
    /// <summary>
    /// Entity for the dbo.genPaycheckHistory table.
    /// </summary>
    public partial class PaycheckHistory : Entity<PaycheckHistory>
    {
        public virtual int       GenPaycheckHistoryId { get; set; } 
        public virtual int       GenPayrollHistoryId  { get; set; } 
        public virtual int       EmployeeId           { get; set; } 
        public virtual string    SubCheck             { get; set; } 
        public virtual DateTime  PeriodEnd            { get; set; } 
        public virtual decimal   CheckAmount          { get; set; } 
        public virtual int?      CheckNumber          { get; set; } 
        public virtual decimal   GrossPay             { get; set; } 
        public virtual decimal   PartialGrossPay      { get; set; } 
        public virtual decimal   Tips                 { get; set; } 
        public virtual decimal   PartialTips          { get; set; } 
        public virtual decimal   NetPay               { get; set; } 
        public virtual bool      IsAdjustment         { get; set; } 
        public virtual bool      IsFicaExempt         { get; set; } 
        public virtual bool      IsFutaExempt         { get; set; } 
        public virtual bool      IsSutaExempt         { get; set; } 
        public virtual bool      IsIncomeTaxExempt    { get; set; } 
        public virtual bool      Is1099Exempt         { get; set; } 
        public virtual decimal   SocSecWages          { get; set; } 
        public virtual decimal   MedicareWages        { get; set; } 
        public virtual decimal   FutaWages            { get; set; } 
        public virtual decimal   MedicareTax          { get; set; } 
        public virtual decimal   EmployerMedicareTax  { get; set; } 
        public virtual decimal   SocSecTax            { get; set; } 
        public virtual decimal   EmployerSocSecTax    { get; set; } 
        public virtual decimal   EmployerFutaTax      { get; set; } 
        public virtual decimal   ExemptWages          { get; set; } 
        public virtual decimal   FlexDeductions       { get; set; } 
        public virtual decimal   TotalTax             { get; set; } 
        public virtual double    StraightHours        { get; set; } 
        public virtual decimal   StraightPay          { get; set; } 
        public virtual double    PremiumHours         { get; set; } 
        public virtual decimal   PremiumPay           { get; set; } 
        public virtual bool      IsLt3Psp             { get; set; } 
        public virtual bool      IsSt3Psp             { get; set; } 
        public virtual decimal   TipCredits           { get; set; } 
        public virtual decimal   HireActWages         { get; set; } 
        public virtual decimal   HireActCredit        { get; set; } 
        public virtual int       ClientId             { get; set; } 
        public virtual DateTime  Modified             { get; set; } 
        public virtual string    ModifiedBy           { get; set; } 
        public virtual decimal?  CustomGrossPay       { get; set; } 
        public virtual DateTime? PayrollCheckDate     { get; set; } 
        public virtual int?      PayrollId            { get; set; } 
        public virtual bool      IsStateTaxExempt     { get; set; } 
        public virtual bool      IsVoid               { get; set; }
        public virtual decimal   DeferredEESocSecTax  { get; set; }

        //REVERSE NAVIGATION
        public virtual ICollection<PaycheckPayDataHistory> PaycheckPayDataHistory { get; set; } // many-to-one;
        public virtual ICollection<PaycheckEarningHistory> EarningHistory { get; set; }

        //FOREIGN KEYS
        public virtual PayrollHistory    PayrollHistory { get; set; }
        public virtual Employee.Employee Employee       { get; set; }
        public virtual Client            Client         { get; set; }
        public virtual Payroll           Payroll        { get; set; }
    }
}