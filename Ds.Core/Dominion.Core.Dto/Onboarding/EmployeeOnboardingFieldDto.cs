using System;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Onboarding
{
    [Serializable]
    public class EmployeeOnboardingFieldDto
    {
        public int EmployeeId { get; set; } 
        public int OnboardingFieldId { get; set; } 
        public string StingValue { get; set; } 
        public bool? IsBoolValue { get; set; } 
        public int? IntValue { get; set; } 
        public decimal? DecimalValue { get; set; }  

        public bool IsNew { get; set; }
       
    }


    [Serializable]
    public class EmployeeOnboardingFieldListDto
    {
        public List<EmployeeOnboardingFieldDto> EmployeeOnboardingFieldList { get; set; }

    }

}
