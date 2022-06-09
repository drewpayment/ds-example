using System;

namespace Dominion.Core.Dto.Onboarding.Forms
{
    public class W4StateILFormDto
    {
        public int EmployeeId { get; set; }
        public string Employee_FirstName { get; set; }
        public string Employee_LastName { get; set; }
        public string Employee_MiddleInitial { get; set; }
        public string Employee_SocialSecurityNumber { get; set; }
        public DateTime? Employee_BirthDate { get; set; }
        public string Employee_AddressLine1 { get; set; }
        public string Employee_AddressLine2 { get; set; }
        public string Employee_City { get; set; }
        public string Employee_StateAbbreviation { get; set; }
        public string Employee_Zipcode { get; set; }
        public bool? IsTaxExempt { get; set; }

        public bool? IsDependentCareClaim { get; set; }
        public bool? IsClaimSpouse { get; set; }
        public int? Step1BoxCount { get; set; }
        public int? DependentsWithStep1BoxCount { get; set; }
        public int? DependentCount { get; set; }
        public int? Allowances { get; set; }

        public bool? AgeOver65
        {
            get { return this.Age >= 65; }
            private set { }
        }
        public bool? IsLegallyBlind { get; set; }
        public bool? IsSpouseOver65 { get; set; }
        public bool? IsSpouseBlind { get; set; }
        public int? Step2BoxCount
        {
            get {
                return (this.AgeOver65.GetValueOrDefault(false) ? 1 : 0) + (this.IsLegallyBlind.GetValueOrDefault(false) ? 1 : 0)
                            + (this.IsSpouseOver65.GetValueOrDefault(false) ? 1 : 0) + (this.IsSpouseBlind.GetValueOrDefault(false) ? 1 : 0);
            }
            private set { }
        }
        public int? FederalSubtractions { get; set; }
        public int? FederalSubtractionsAndAllowances
        {
            get
            {
                return (this.Allowances.GetValueOrDefault(0) + this.FederalSubtractions.GetValueOrDefault(0));
            }
            private set { }
        }
        public int? Line7 {
            get {
                return (int)Math.Round((decimal)this.FederalSubtractionsAndAllowances.GetValueOrDefault(0) / 1000);
            }
            private set { }
        }
        public int? Line8 {
            get
            {
                return this.Step2BoxCount.GetValueOrDefault(0) + this.Line7.GetValueOrDefault(0);
            }
            private set { }
        }
        public int? AdditionalExcemptions { get; set; }
        public int? AdditionalWithHolding { get; set; }

        public string SignatureName { get; set; }
        public DateTime? SignatureDate { get; set; }
        public int Age { get; set; }
    }
}
