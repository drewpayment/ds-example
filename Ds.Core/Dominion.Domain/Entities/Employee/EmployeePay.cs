using System;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Employee
{
    /// <summary>
    /// Entity definition of an employee's pay settings.
    /// </summary>
    public class EmployeePay : Entity<EmployeePay>, IEmployeePay, IHasModifiedOptionalData
    {
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
        public virtual DateTime?          Modified                    { get; set; }
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
        public virtual bool               IsArpExempt                 { get; set; }
        public virtual bool               StateTaxExempt              { get; set; }
        public virtual int?               WotcReasonId                { get; set; }
        public virtual bool               IsCobraParticipant          { get; set; }
        public virtual int?               EmployeeTerminationReasonId { get; set; }
        public virtual RehireEligibleType? RehireEligibleId           { get; set; }
        public virtual bool               DeferEESocSecTax            { get; set; }


        public virtual ClockEmployee                ClockEmployee               { get; set; }
        public virtual Employee                     Employee                    { get; set; }
        public virtual EmployeeStatus               EmployeeStatus              { get; set; }
        public virtual PayFrequency                 PayFrequency                { get; set; }
        public virtual EmployeePayTypeInfo          PayTypeInfo                 { get; set; }
        public virtual EmployeeTerminationReason    EmployeeTerminationReasonE  { get; set; }
    }
}