using System;

namespace Dominion.Core.Dto.Onboarding.Forms
{
    /// <summary>
    /// DTO containing a 1-to-1 mapping to the Federal W4 form fields.
    /// </summary>
    public class W4FederalFormDto
    {
        public int       FormYear                                 { get; set; }
        public int       EmployeeId                               { get; set; }
        public virtual bool? IsTaxExemptionLastYr { get; set; }
        public virtual bool? IsTaxExemptionCurrYr { get; set; }
        public int?      PersonalAllowance_A_Self                 { get; set; }
        public int?      PersonalAllowance_B_SelfAdjustment       { get; set; }
        public int?      PersonalAllowance_C_Spouse               { get; set; }
        public int?      PersonalAllowance_D_Depedents            { get; set; }
        public int?      PersonalAllowance_E_HeadOfHousehold      { get; set; }
        public int?      PersonalAllowance_F_DependentCare        { get; set; }
        public int?      PersonalAllowance_G_ChildTaxCredit       { get; set; }
        public int?      PersonalAllowance_H_TotalAllowances      { get; set; }
        public string    W4_FirstName                             { get; set; }
        public string    W4_MiddleInitial                         { get; set; }
        public string    W4_LastName                              { get; set; }
        public string    W4_SocialSecurityNumber                  { get; set; }
        public string    W4_HomeAddressLine1                      { get; set; }
        public string    W4_HomeAddressLine2                      { get; set; }
        public string    W4_City                                  { get; set; }
        public string    W4_StateAbbreviation                     { get; set; }
        public string    W4_Zipcode                               { get; set; }
        public bool      W4_Withholding_IsSingle                  { get; set; }
        public bool      W4_Withholding_IsMarried                 { get; set; }
        public bool      W4_Withholding_IsMarriedFilingSingle     { get; set; }
        public bool?      W4_IsLastNameDifferentFromSocialSecurity { get; set; }
        public int       W4_TotalNumberOfAllowances               { get; set; }
        public decimal?  W4_AdditionalAmountToWitholdPerPay       { get; set; }
        public bool      W4_IsExempt                              { get; set; }
        public string    W4_EmployerNameAndAddress                { get; set; }
        public string    W4_OfficeCode                            { get; set; }
        public string    W4_EmployerIdentificationNumber          { get; set; }
        public DateTime? W4_FirstDateOfEmployment { get; set; }
        public string    SignatureName                            { get; set; }
        public DateTime? SignatureDate                            { get; set; }
        public decimal?  Deductions_01                            { get; set; }
        public decimal?  Deductions_02                            { get; set; }
        public decimal?  Deductions_03                            { get; set; }
        public decimal?  Deductions_04                            { get; set; }
        public decimal?  Deductions_05                            { get; set; }
        public decimal?  Deductions_06                            { get; set; }
        public decimal?  Deductions_07                            { get; set; }
        public decimal?  Deductions_08                            { get; set; }
        public decimal?  Deductions_09                            { get; set; }
        public decimal?  Deductions_10                            { get; set; }
        public decimal?  MultiEarner_01                           { get; set; }
        public decimal?  MultiEarner_02                           { get; set; }
        public decimal?  MultiEarner_03                           { get; set; }
        public decimal?  MultiEarner_04                           { get; set; }
        public decimal?  MultiEarner_05                           { get; set; }
        public decimal?  MultiEarner_06                           { get; set; }
        public decimal?  MultiEarner_07                           { get; set; }
        public decimal?  MultiEarner_08                           { get; set; }
        public decimal?  MultiEarner_09                           { get; set; }
    }
}
