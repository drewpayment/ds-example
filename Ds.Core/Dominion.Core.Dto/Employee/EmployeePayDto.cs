using System;
using Dominion.Core.Dto.Payroll;
using Dominion.Utility.Dto;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeePayDto : DtoObject
    {
        public int                EmployeeId                { get; set; }
        public PayFrequencyType   PayFrequencyId            { get; set; }
        public PayType?           Type                      { get; set; }
        public double?            SalaryAmount              { get; set; }
        public DateTime?          SalaryAmountEffectiveDate { get; set; }
        public double?            Hours                     { get; set; }
        public EmployeeStatusType EmployeeStatusId          { get; set; }
        public string             EmployeeStatusDescription { get; set; }
        public int?               ClientShiftId             { get; set; }
        public int?               ClientTaxId               { get; set; }
        public DateTime?          Modified                  { get; set; }
        public int?               ModifiedBy                { get; set; }
        public int                ClientId                  { get; set; }
        public string             AcaNote                   { get; set; }
	    public int                EmployeePayId             { get; set; }
        public int                EmployeeTerminationReason { get; set; }
        public int                RehireEligible            { get; set; }
        public bool?              IsTippedEmployee          { get; set; }
        public bool StateTaxExempt { get; set; }
        public int? WotcReasonId { get; set; }
        public bool DeferEESocSecTax { get; set; }
        public bool IsFicaExempt { get; set; }
        public bool IsFutaExempt { get; set; }
        public bool IsSutaExempt { get; set; }
        public bool IsIncomeTaxExempt { get; set; }
        public bool IsC1099Exempt { get; set; }
        public bool IsSocSecExempt { get; set; }
        public bool IsHireActQualified { get; set; }
        public DateTime HireActStartDate { get; set; }
        public bool IsArpExempt { get; set; }
    }
}
