using System;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Payroll;

namespace Dominion.Domain.Entities.Employee
{
    /// <summary>
    /// Represents an employee's pay settings.
    /// </summary>
    public interface IEmployeePay
    {
        int                EmployeePayId               { get; set; }
        int                EmployeeId                  { get; set; }
        PayFrequencyType   PayFrequencyId              { get; set; }
        PayType?           Type                        { get; set; }
        double?            ContractAmount              { get; set; }
        DateTime?          ContractAmountEffectiveDate { get; set; }
        double?            SalaryAmount                { get; set; }
        DateTime?          SalaryAmountEffectiveDate   { get; set; }
        double?            Hours                       { get; set; }
        EmployeeStatusType EmployeeStatusId            { get; set; }
        int?               ClientShiftId               { get; set; }
        int?               ClientTaxId                 { get; set; }
        bool               IsFicaExempt                { get; set; }
        bool               IsFutaExempt                { get; set; }
        bool               IsSutaExempt                { get; set; }
        bool               IsIncomeTaxExempt           { get; set; }
        bool               IsC1099Exempt               { get; set; }
        bool               IsSocSecExempt              { get; set; }
        bool?              IsTippedEmployee            { get; set; }
        bool               IsHireActQualified          { get; set; }
        double?            TempAgencyPercent           { get; set; }
        double?            TempAgencyPercentOtOverride { get; set; }
        double?            TempAgencyPercentDtOverride { get; set; }
        int                ClientId                    { get; set; }
        DateTime           HireActStartDate            { get; set; }
        string             SalaryNote                  { get; set; }
        string             ContractNote                { get; set; }
        bool               IsExcludeFromAca            { get; set; } 
        string             AcaNote                     { get; set; } 

        Employee           Employee                    { get; set; }
        EmployeeStatus     EmployeeStatus              { get; set; }
        PayFrequency       PayFrequency                { get; set; }
    }
}
