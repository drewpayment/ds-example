using System.Collections.Generic;

namespace Dominion.Domain.Entities.Payroll
{
    /// <summary>
    /// Entity representation of a payroll preview paycheck's pay summary info.  Details can be found in sub-entities
    /// such as associated earnings and deductions.
    /// </summary>
    public partial class PreviewPaycheckPayData
    {
        public virtual int      PreviewPaycheckPayDataId { get; set; } 
        public virtual int      PreviewPaycheckId        { get; set; } 
        public virtual int?     PayrollPayDataId         { get; set; } 
        public virtual decimal  GrossPay                 { get; set; } 
        public virtual decimal  Tips                     { get; set; } 
        public virtual decimal  PartialGrossPay          { get; set; } 
        public virtual decimal  PartialTips              { get; set; } 
        public virtual double   PercentOfCheck           { get; set; } 
        public virtual decimal  TotalTax                 { get; set; } 
        public virtual int?     AppliesToPayrollId       { get; set; } 
        public virtual int?     ClientWorkersCompId      { get; set; } 
        public virtual int?     ClientGroupId            { get; set; } 
        public virtual int?     ClientDivisionId         { get; set; } 
        public virtual int?     ClientDepartmentId       { get; set; } 
        public virtual int?     SutaStateClientTaxId     { get; set; } 
        public virtual int?     ClientCostCenterId       { get; set; } 
        public virtual int?     ClientShiftId            { get; set; } 
        public virtual decimal  CheckAmount              { get; set; } 
        public virtual decimal  NetPay                   { get; set; } 
        public virtual decimal  SocSecWages              { get; set; } 
        public virtual decimal  MedicareWages            { get; set; } 
        public virtual decimal  FutaWages                { get; set; } 
        public virtual decimal  MedicareTax              { get; set; } 
        public virtual decimal  EmployerMedicareTax      { get; set; } 
        public virtual decimal  SocSecTax                { get; set; } 
        public virtual decimal  EmployerSocSecTax        { get; set; } 
        public virtual decimal  EmployerFutaTax          { get; set; } 
        public virtual decimal  ExemptWages              { get; set; } 
        public virtual decimal  FlexDeductions           { get; set; } 
        public virtual double   StraightHours            { get; set; } 
        public virtual decimal  StraightPay              { get; set; } 
        public virtual double   PremiumHours             { get; set; } 
        public virtual decimal  PremiumPay               { get; set; } 
        public virtual decimal  TipCredits               { get; set; } 
        public virtual decimal  AdjustToNet              { get; set; } 
        public virtual decimal  HireActWages             { get; set; } 
        public virtual decimal  HireActCredit            { get; set; } 
        public virtual int      ClientId                 { get; set; } 
        public virtual int      EmployeeId               { get; set; } 
        public virtual decimal? SutaLimitWages           { get; set; } 
        public virtual decimal? SutaExcessWages          { get; set; } 
        public virtual decimal? CustomGrossPay           { get; set; }
        public virtual decimal  DeferredEESocSecTax      { get; set; }

        public virtual ICollection<PreviewPaycheckEarning> PreviewPaycheckEarning { get; set; } 

        public virtual PreviewPaycheck PreviewPaycheck { get; set; } 
    }
}
