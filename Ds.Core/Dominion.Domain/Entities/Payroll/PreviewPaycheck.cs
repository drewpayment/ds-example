using System;
using System.Collections.Generic;

namespace Dominion.Domain.Entities.Payroll
{
    /// <summary>
    /// Entity representation of the a payroll preview paycheck.
    /// </summary>
    public partial class PreviewPaycheck
    {
        public virtual int      PreviewPaycheckId           { get; set; } 
        public virtual int      PayrollId                   { get; set; } 
        public virtual int      EmployeeId                  { get; set; } 
        public virtual string   SubCheck                    { get; set; } 
        public virtual DateTime PeriodEnd                   { get; set; } 
        public virtual decimal  CheckAmount                 { get; set; } 
        public virtual int?     CheckNumber                 { get; set; } 
        public virtual decimal  GrossPay                    { get; set; } 
        public virtual decimal  PartialGrossPay             { get; set; } 
        public virtual decimal  Tips                        { get; set; } 
        public virtual decimal  PartialTips                 { get; set; } 
        public virtual decimal  NetPay                      { get; set; } 
        public virtual bool     IsAdjustment                { get; set; } 
        public virtual bool     IsFicaExempt                { get; set; } 
        public virtual bool     IsFutaExempt                { get; set; } 
        public virtual bool     IsSutaExempt                { get; set; } 
        public virtual bool     IsIncomeTaxExempt           { get; set; } 
        public virtual bool     Is1099Exempt                { get; set; } 
        public virtual decimal  SocSecWages                 { get; set; } 
        public virtual decimal  MedicareWages               { get; set; } 
        public virtual decimal  FutaWages                   { get; set; } 
        public virtual decimal  MedicareTax                 { get; set; } 
        public virtual decimal  EmployerMedicareTax         { get; set; } 
        public virtual decimal  SocSecTax                   { get; set; } 
        public virtual decimal  EmployerSocSecTax           { get; set; } 
        public virtual decimal  EmployerFutaTax             { get; set; } 
        public virtual decimal  ExemptWages                 { get; set; } 
        public virtual decimal  FlexDeductions              { get; set; } 
        public virtual decimal  TotalTax                    { get; set; } 
        public virtual double   StraightHours               { get; set; } 
        public virtual decimal  StraightPay                 { get; set; } 
        public virtual double   PremiumHours                { get; set; } 
        public virtual decimal  PremiumPay                  { get; set; } 
        public virtual bool     IsLt3Psp                    { get; set; } 
        public virtual bool     IsSt3Psp                    { get; set; } 
        public virtual decimal  TipCredits                  { get; set; } 
        public virtual decimal  AdjustToNet                 { get; set; } 
        public virtual int?     TaxFactorId                 { get; set; } 
        public virtual bool?    IsIncludeInYtd              { get; set; } 
        public virtual decimal  HireActWages                { get; set; } 
        public virtual decimal  HireActCredit               { get; set; } 
        public virtual bool?    IsVoidCheck                 { get; set; } 
        public virtual int?     GenPaycheckHistoryId        { get; set; } 
        public virtual decimal  CustomGrossPay              { get; set; } 
        public virtual double   CurrentPayrollLifetimeHours { get; set; } 
        public virtual int      ClientId                    { get; set; }
        public virtual decimal DeferredEESocSecTax          { get; set; }

        public virtual ICollection<PreviewPaycheckPayData> PreviewPaycheckPayData { get; set; }
    }
}
