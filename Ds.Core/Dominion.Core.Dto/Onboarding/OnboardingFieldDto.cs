using System;


namespace Dominion.Core.Dto.Onboarding
{
    [Serializable]
    public class OnboardingFieldDto
    {
        public virtual int    OnboardingFieldId { get; set; } 
        public virtual int    EmployeeOnboardingWorkflowTask { get; set; } 
        public virtual string Description { get; set; } 
        public virtual int    FieldTypeId { get; set; } 
       
    }
}
