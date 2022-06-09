using System;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Payroll;

namespace Dominion.Domain.Entities.Employee
{
    /// <summary>
    /// Historical change record of an <see cref="EmployeePay"/>.
    /// </summary>
    public class EmployeePayChangeHistory : Entity<EmployeePayChangeHistory>, IEmployeePay
    {
        public virtual int                ChangeId                    { get; set; } 
        public virtual int                EmployeePayId               { get; set; } 
        public virtual int                EmployeeId                  { get; set; } 
        public virtual PayFrequencyType   PayFrequencyId              { get; set; } 
        public virtual PayType?           Type                        { get; set; } 
        public virtual double?            ContractAmount              { get; set; } 
        public virtual DateTime?          ContractAmountEffectiveDate { get; set; } 
        public virtual double?            SalaryAmount                { get; set; } 
        public virtual DateTime?          SalaryAmountEffectiveDate   { get; set; } 
        public virtual double?            Hours                       { get; set; } 
        public virtual EmployeeStatusType EmployeeStatusId            { get; set; } 
        public virtual int?               ClientShiftId               { get; set; } 
        public virtual int?               ClientTaxId                 { get; set; } 
        public virtual bool               IsFicaExempt                { get; set; } 
        public virtual bool               IsFutaExempt                { get; set; } 
        public virtual bool               IsSutaExempt                { get; set; } 
        public virtual bool               IsIncomeTaxExempt           { get; set; } 
        public virtual bool               IsC1099Exempt               { get; set; } 
        public virtual bool               IsSocSecExempt              { get; set; } 
        public virtual DateTime           ChangeDate                  { get; set; } 
        public virtual int?               ModifiedBy                  { get; set; } 
        public virtual bool?              IsTippedEmployee            { get; set; } 
        public virtual bool               IsHireActQualified          { get; set; } 
        public virtual double?            TempAgencyPercent           { get; set; } 
        public virtual double?            TempAgencyPercentOtOverride { get; set; } 
        public virtual double?            TempAgencyPercentDtOverride { get; set; } 
        public virtual int                ClientId                    { get; set; } 
        public virtual DateTime           HireActStartDate            { get; set; } 
        public virtual string             SalaryNote                  { get; set; } 
        public virtual string             ContractNote                { get; set; } 
        public virtual bool               IsExcludeFromAca            { get; set; } 
        public virtual string             AcaNote                     { get; set; }
        public virtual bool               DeferEESocSecTax            { get; set; }
 
        public virtual Employee           Employee                    { get; set; }
        public virtual EmployeeStatus     EmployeeStatus              { get; set; }
        public virtual PayFrequency       PayFrequency                { get; set; }
    }
}
