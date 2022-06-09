using System;

namespace Dominion.Core.Dto.Onboarding.Forms
{
    public class W4StateINFormDto
    {
        public int EmployeeId { get; set; }
        public string Employee_FirstName { get; set; }// Full name in pdf
        public string Employee_LastName { get; set; }// Full name in pdf
        public string Employee_MiddleInitial { get; set; }// Full name in pdf
        public string Employee_SocialSecurityNumber { get; set; }//SSN or ITIN in pdf


        public string Employee_AddressLine1 { get; set; }//Home address in PDF
        public string Employee_AddressLine2 { get; set; }//Home address in PDF
        public string Employee_City { get; set; }//City in pdf
        //todoram:DOubt if the state in pdf should be full state name or just abbrevation. In PDF if Full state nae is given it overflows in PDF
        public string Employee_StateAbbreviation { get; set; }//STate in pdf
        public string Employee_Zipcode { get; set; }// ZIp Code in PDF

        public DateTime? Employee_BirthDate { get; set; }// BirthDate for Age calculation
        public string Employee_CountyOfResidence { get; set; }// County of residence in PDF
        public string Employee_CountyOfEmployment { get; set; }// County of Employment in PDF
        public bool? IsEmployeeBlind { get; set; }

        public bool? IsEmployeeOver65 { get; set; }
        public bool? IsSpouseBlind { get; set; }
        public bool? IsSpouseOver65 { get; set; }

        public int EmployeeAge { get; set; }
        public int? AdditionalExemptions { get; set; }
        public int? AdditionalWithholdingAmt { get; set; }
        public int? AdditionalCountyWithholdingAmt { get; set; }
        public int? DefaultExemption_Point1 { get; set; }
        public int? MarriedAndSpouseNoClaim_Point2 { get; set; }

       
        public int? NumberOfDependentsExemption_Point3 { get; set; }
        public int? AdditionalExemptionsYouSpouseBlingOrOver65_Point4 { get; set; }
        public int? TotalNumberOfExemptions_Point5 { get; set; }
        public int? AdditionalExemptionsForQualifyingDependents_Point6 { get; set; }
        public decimal? AdditionalStateWithholding_Point7 { get; set; }
        public decimal? AdditionalCountyWithholding_Point8 { get; set; }

        public string SignatureName { get; set; }
        public DateTime? SignatureDate { get; set; }

        public bool IsEmployeeMarried { get; set; }
        public bool? IsSpouseEmployed { get; set; }
        public bool IsAuthorisedAlien{ get; set; }
        public bool IsSpouseExemptionClaimed { get; set; }

        //public int Exemptions_StateW4 { get; set; }


    }
}
