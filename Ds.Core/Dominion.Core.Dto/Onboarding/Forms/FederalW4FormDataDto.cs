using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Taxes.Dto.TaxOptions;

namespace Dominion.Core.Dto.Onboarding.Forms
{
    public class FederalW4FormDataDto
    {
                    public int? EmployeeId { get; set; }

                    public string EmployeeFirstName { get; set; }
                    public string EmployeeLastName { get; set; }
                    public string EmployeeMiddleInitial { get; set; }
                    public string EmployeeSsn { get; set; }
                    public string EmployeeAddressLine1 { get; set; }
                    public string EmployeeAddressLine2 { get; set; }
                    public string EmployeeCity { get; set; }
                    public string EmployeeState { get; set; }
                    public string EmployeeZipcode { get; set; }

                    public string EmployerName { get; set; }
                    public string EmployerAddressLine1 { get; set; }
                    public string EmployerAddressLine2  { get; set; }
                    public string EmployerCity { get; set; }
                    public string EmployerStateAbbreviation { get; set; }
                    public string EmployerZipcode { get; set; }
                    public string EmployerEin  { get; set; }
                    public bool?   IsTaxExemptionCurrYr  { get; set; }
                    public bool?   IsTaxExemptionLastYr  { get; set; }

                    public int? AdditionalWithholdingAmt  { get; set; }
                    public bool? IsAdditionalAmountWithheld { get; set; }
                    public int? Allowances  { get; set; }
                    public byte? FilingStatus  { get; set; }
                    public bool? IsTaxExempt  { get; set; }

                    public byte? ChildCount  { get; set; }
                    public byte? DependentCount  { get; set; }
                    public W4MaritalStatus? MaritalStatus { get; set; }
                    public byte? HouseholdIncomeStatus { get; set; }
                    public bool? HasChildren { get; set; }
                    public bool? HasDependents { get; set; }
                    public bool? IsDependentCareOverLimit { get; set; }
                    public bool? IsDependentCareClaim { get; set; }
                    public bool? IsDependentOtherReturn  { get; set; }
                    public bool? IsSpouseEmployed { get; set; }
                    public bool? IsWidowerCurrYr  { get; set; }
                    public bool? DoYouHaveAnotherJob  { get; set; }
                    public bool? SecondJobAndSpouseEarnMoreThan1500  { get; set; }
                    public bool? DependentOrChildCareExpensesIsMoreThan2000 { get; set; }
                    public int? ChildTaxCredit { get; set; }
                    public FilingStatus? TaxFilingStatus  { get; set; }
                    public bool? IsNameDifferFromSSC  { get; set; }
                    public bool W4_HasTwoSimilarJobs { get; set; }
                    public decimal W4_QualifyingChildrenAmount { get; set; }
                    public decimal W4_OtherDependentsAmount { get; set; }
                    public decimal W4_TaxCredit { get; set; }
                    public decimal W4_OtherTaxableIncome { get; set; }
                    public decimal W4_WageDeduction { get; set; }
                    public decimal? W4_AdditionalAmountToWitholdPerPay { get; set; }
                    public DateTime? W4_FirstDateOfEmployment { get; set; }
                    public bool W4_Withholding_IsSingleOrMarriedFilingSeperately { get; set; }


    }
}
