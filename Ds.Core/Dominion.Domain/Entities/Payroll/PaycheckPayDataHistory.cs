using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Payroll
{
    /// <summary>
    /// Entity for the dbo.genPaycheckPayDataHistory table.
    /// </summary>
    public partial class PaycheckPayDataHistory : Entity<PaycheckPayDataHistory>
    {
        public virtual int       GenPaycheckPayDataHistoryId { get; set; } 
        public virtual int       GenPaycheckHistoryId        { get; set; } 
        public virtual int?      PayrollPayDataId            { get; set; } 
        public virtual decimal   GrossPay                    { get; set; } 
        public virtual decimal   PartialGrossPay             { get; set; } 
        public virtual decimal   Tips                        { get; set; } 
        public virtual decimal   PartialTips                 { get; set; } 
        public virtual double    PercentOfCheck              { get; set; } 
        public virtual decimal   TotalTax                    { get; set; } 
        public virtual int?      AppliesToPayrollId          { get; set; } 
        public virtual int?      ClientWorkersCompId         { get; set; } 
        public virtual int?      ClientGroupId               { get; set; } 
        public virtual int?      ClientDivisionId            { get; set; } 
        public virtual int?      ClientDepartmentId          { get; set; } 
        public virtual int?      SutaStateClientTaxId        { get; set; } 
        public virtual int?      ClientCostCenterId          { get; set; } 
        public virtual int?      ClientShiftId               { get; set; } 
        public virtual decimal   CheckAmount                 { get; set; } 
        public virtual decimal   NetPay                      { get; set; } 
        public virtual decimal   SocSecWages                 { get; set; } 
        public virtual decimal   MedicareWages               { get; set; } 
        public virtual decimal   FutaWages                   { get; set; } 
        public virtual decimal   MedicareTax                 { get; set; } 
        public virtual decimal   EmployerMedicareTax         { get; set; } 
        public virtual decimal   SocSecTax                   { get; set; } 
        public virtual decimal   EmployerSocSecTax           { get; set; } 
        public virtual decimal   EmployerFutaTax             { get; set; } 
        public virtual decimal   ExemptWages                 { get; set; } 
        public virtual decimal   FlexDeductions              { get; set; } 
        public virtual double    StraightHours               { get; set; } 
        public virtual decimal   StraightPay                 { get; set; } 
        public virtual double    PremiumHours                { get; set; } 
        public virtual decimal   PremiumPay                  { get; set; } 
        public virtual decimal   TipCredits                  { get; set; } 
        public virtual decimal   HireActWages                { get; set; } 
        public virtual decimal   HireActCredit               { get; set; } 
        public virtual int       ClientId                    { get; set; } 
        public virtual int       EmployeeId                  { get; set; } 
        public virtual DateTime  Modified                    { get; set; } 
        public virtual string    ModifiedBy                  { get; set; } 
        public virtual decimal?  CustomGrossPay              { get; set; } 
        public virtual DateTime? PayrollCheckDate            { get; set; } 
        public virtual int?      PayrollId                   { get; set; }
        public virtual decimal   DeferredEESocSecTax         { get; set; }

        //REVERSE NAVIGATION
        public virtual ICollection<PaycheckDeductionHistory> PaycheckDeductionHistory { get; set; } // many-to-one;
        public virtual ICollection<PaycheckEarningHistory> PaycheckEarningHistory { get; set; } // many-to-one;
        public virtual ICollection<PaycheckTaxHistory> PaycheckTaxHistory { get; set; } // many-to-one;
        public virtual ICollection<PaycheckSUTAHistory> PaycheckSutaHistory { get; set; } // many-to-one;

        //FOREIGN KEYS
        public virtual PaycheckHistory   PaycheckHistory  { get; set; } 
        public virtual PayrollPayData    PayrollPayData   { get; set; }
        public virtual Payroll           Payroll          { get; set; }
        public virtual Employee.Employee Employee         { get; set; }
        public virtual Client            Client           { get; set; }
        public virtual ClientDivision    ClientDivision   { get; set; }
        public virtual ClientDepartment  ClientDepartment { get; set; }
        public virtual ClientCostCenter  ClientCostCenter { get; set; }
    }
}