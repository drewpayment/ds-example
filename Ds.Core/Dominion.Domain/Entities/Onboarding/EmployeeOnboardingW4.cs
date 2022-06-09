using System;

using Dominion.Core.Dto.Onboarding;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Taxes.Dto.TaxTypes;
using Dominion.Domain.Entities.Misc;

namespace Dominion.Domain.Entities.Onboarding
{
    public  class EmployeeOnboardingW4 : Entity<EmployeeOnboardingW4>, IHasModifiedData
    {
        public virtual int              EmpTaxId                     { get; set; }
        public virtual int              EmployeeId                   { get; set; }
        public virtual TaxCategory      TaxCategory                  { get; set; }
        public virtual int              StateId                      { get; set; }
        public virtual string           LocalCode                    { get; set; }
        public virtual bool?            IsTaxExempt                  { get; set; }
        public virtual bool? IsTaxExemptionLastYr { get; set; }
        public virtual bool? IsTaxExemptionCurrYr { get; set; }
        public virtual TaxExemptReason? TaxExemptReasonId            { get; set; }
        public virtual int?             Allowances                   { get; set; }
        public virtual byte?            FilingStatus                 { get; set; }
        public virtual bool?            IsAdditionalAmountWithheld   { get; set; }
        public virtual int?             AdditionalWithholdingAmt     { get; set; }
        
        
        public virtual bool?            IsResident                   { get; set; }
        public virtual string           RenaissanceZone              { get; set; }
        public virtual string           TaxExemptReason              { get; set; }      
        public virtual DateTime         CreateDt                     { get; set; }
        public virtual DateTime         Modified                     { get; set; }
        public virtual int              ModifiedBy                   { get; set; }
        public virtual int?             CountyId                     { get; set; }
        public virtual int?             EmploymentCountyId           { get; set; }
        public virtual int?             AdditionalExemptions         { get; set; }
        public virtual int?             AdditionalCountyWithholdingAmt { get; set; }
        public virtual int?             SchoolDistrictId             { get; set; }
        public virtual bool?            IsFederalSubtractions       { get; set; }
        public virtual int?             FederalSubtractions         { get; set; }

        public virtual double?          AllowableDeductionsToFedAdjGrossIncome { get; set; }
        public virtual double?          IncomeNotSubjectToWithholding { get; set; }
		public virtual int?             EstimatedDeductionAllowances { get; set; }
		public virtual double?          EstimatedItemizedDeductions { get; set; }
        public virtual int?             ReciprocalStateId { get; set; }
        public virtual bool?            WithheldTaxAtLowerRate { get; set; }

        public virtual bool? IsAdditionalCountyAmountWithheld { get; set; }
        public virtual bool? IsIndianaResident { get; set; }
        public virtual bool? IsEmployedInIndiana { get; set; }
        public virtual Employee.Employee          Employee        { get; set; }
        public virtual EmployeeOnboardingW4Assist W4AssistantInfo { get; set; }
        public virtual SchoolDistrict SchoolDistrict { get; set; }

        public virtual County County { get; set; }
        public virtual County CountyOfEmployment { get; set; }
        public virtual State Reciprocal_State { get; set; }
        public virtual int QualifyingChildren { get; set; }
        public virtual int OtherDependents { get; set; }
        public virtual decimal OtherTaxableIncome { get; set; }
        public virtual decimal WageDeduction { get; set; }
        public virtual bool HasMoreThanOneJob { get; set; }
        public virtual bool Using2020FederalW4Setup { get; set; }
        public virtual decimal QualifyingChildrenAmount { get; set; }
        public virtual decimal OtherDependentsAmount { get; set; }
        public virtual decimal TaxCredit { get; set; }
        public virtual bool IsSpouseOver65 { get; set; }
        public virtual bool IsBlind { get; set; }
        public virtual bool IsSpouseBlind { get; set; }
        public virtual bool IsSpouseEmployed { get; set; }
        public virtual int TotalNumberOfDependents { get; set; }
        public virtual bool IsClaimedAsDependent { get; set; }
        public virtual bool IsClaimingSpouseExemption { get; set; }
        public virtual bool IsMarriedFilingSeparately { get; set; }
        public virtual bool IsPersonalExemption { get; set; }
        public virtual bool? IsResidentOfNewYorkCity { get; set; }
        public virtual bool? IsAdditionalNYCAmountWithheld { get; set; }
        public virtual int? NYCAdditionalWithholdingAmt { get; set; }
        public virtual int? NYCAllowances { get; set; }
        public virtual bool? IsResidentOfYonkers { get; set; }
        public virtual bool? IsAdditionalYonkersAmountWithheld { get; set; }
        public virtual int? YonkersAdditionalWithholdingAmt { get; set; }
		
        public virtual decimal? TaxableWagesPercentage { get; set; }
		public virtual int? InstructionAOptionFromChart { get; set; }

    }
}
