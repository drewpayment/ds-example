using System;

using Dominion.Taxes.Dto.TaxTypes;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Onboarding
{
    [Serializable]
    public class EmployeeOnboardingW4Dto
    {
        public virtual int? EmpTaxId { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual TaxCategory TaxCategory { get; set; }
        public virtual int? StateId { get; set; }
        public virtual string LocalCode { get; set; }
        public virtual bool? IsTaxExempt { get; set; }
        public virtual bool? IsTaxExemptionLastYr { get; set; }
        public virtual bool? IsTaxExemptionCurrYr { get; set; }
        public virtual TaxExemptReason? TaxExemptReasonId { get; set; }
        public virtual int? Allowances { get; set; }
        public virtual byte? FilingStatus { get; set; }
        public virtual bool? IsAdditionalAmountWithheld { get; set; }
        public virtual int? AdditionalWithholdingAmt { get; set; }

        public virtual bool? IsResident { get; set; }
        public virtual string RenaissanceZone { get; set; }
        public virtual string TaxExemptReason { get; set; }
        public virtual int? TotalExemptions { get; set; }
        public virtual DateTime CreateDt { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual int? CountyId { get; set; }
        public virtual int? EmploymentCountyId { get; set; }
        public virtual int? AdditionalExemptions { get; set; }
        public virtual int? AdditionalCountyWithholdingAmt { get; set; }
        public virtual bool? IsIndianaResident { get; set; }
        public virtual bool? IsEmployedInIndiana { get; set; }
        public virtual int? SchoolDistrictId { get; set; }

        public virtual bool? IsAdditionalCountyAmountWithheld { get; set; }
		public virtual List<SchoolDistrictDto> SchoolDistricts { get; set; }

        public virtual bool? IsFederalSubtractions { get; set; }
        public virtual int? FederalSubtractions { get; set; }

        public double? AllowableDeductionsToFedAdjGrossIncome { get; set; }
        public double? IncomeNotSubjectToWithholding { get; set; }
		public double? EstimatedItemizedDeductions { get; set; }
		public int? EstimatedDeductionAllowances { get; set; }
        public virtual  int? ReciprocalStateId { get; set; }
        public virtual bool? WithheldTaxAtLowerRate { get; set; }

        public virtual DateTime? BirthDate { get; set; }
        public virtual int Age {
            get
            {
                return this.BirthDate.HasValue ? this.BirthDate.Value > DateTime.Now.AddYears((DateTime.Now.Year - this.BirthDate.Value.Year) * -1) ?
                                     (DateTime.Now.Year - this.BirthDate.Value.Year) - 1 : DateTime.Now.Year - this.BirthDate.Value.Year : 0;

            }

            private set
            {

            }
       }

        public virtual int RecommendedAllowanceCount
        {
            get
            {
                int count = 0;
                int recommendation = 0;

                if (this.Age > 65)
                {
                    count++;
                }

                if (this.IsSpouseOver65)
                {
                    count++;
                }

                if (this.IsBlind)
                {
                    count++;
                }

                if (this.IsSpouseBlind)
                {
                    count++;
                }

                if (this.FederalSubtractions.GetValueOrDefault(0) > 0)
                {
                    recommendation = (int)Math.Round((double)this.FederalSubtractions / 1000) + count;
                }
                else
                {
                    recommendation = count;
                }

                return recommendation;
            }
            private set { }
        }

        //Fields added for 2020 FederalW4 Changes
        public virtual decimal TaxCredit { get; set; }

        //next 2 are used to calculate EmployeeTax.TaxCredit = QualifyingChildren*2000 + OtherDependents*500
        public virtual int QualifyingChildren { get; set; }
        public virtual decimal QualifyingChildrenAmount { get; set; }
        public virtual int OtherDependents { get; set; } 
        public virtual decimal OtherDependentsAmount { get; set; }
        public virtual decimal OtherTaxableIncome { get; set; }
        public virtual decimal WageDeduction { get; set; }
        public virtual bool HasMoreThanOneJob { get; set; }
        public virtual bool Using2020FederalW4Setup { get; set; }

        //StateW4 Changes - PAY-1001
        public virtual bool IsSpouseOver65 { get; set; }
        public virtual bool IsBlind { get; set; }
        public virtual bool IsSpouseBlind { get; set; }
        public virtual bool IsSpouseEmployed { get; set; }
        public virtual int TotalNumberOfDependents { get; set; }
        public virtual bool IsClaimedAsDependent { get; set; }
        public virtual bool IsClaimingSpouseExemption { get; set; }
        public virtual bool IsMarriedFilingSeparately { get; set; }
        public virtual bool IsPersonalExemption { get; set; }
        public virtual decimal? TaxableWagesPercentage { get; set; }
		
        //StateW4 NY Changes - ON-841
        public virtual bool? IsResidentOfNewYorkCity { get; set; }
        public virtual bool? IsAdditionalNYCAmountWithheld { get; set; }
        public virtual int? NYCAdditionalWithholdingAmt { get; set; }
        public virtual int? NYCAllowances { get; set; }

        public virtual bool? IsResidentOfYonkers { get; set; }
        public virtual bool? IsAdditionalYonkersAmountWithheld { get; set; }
        public virtual int? YonkersAdditionalWithholdingAmt { get; set; }
		
        //StateW4 NJ Changes - ON-842
        public virtual int? InstructionAOptionFromChart { get; set; }
    }
}
