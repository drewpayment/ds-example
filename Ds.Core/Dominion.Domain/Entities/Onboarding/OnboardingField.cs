using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Onboarding
{
    public  class OnboardingField : Entity<OnboardingField>
    {
		public virtual int OnboardingFieldId { get; set; } 
		public virtual int EmployeeOnboardingWorkflowTask { get; set; } 
		public virtual string Description { get; set; } 
		public virtual int FieldTypeId { get; set; } 

		//REVERSE NAVIGATION
		public virtual ICollection<EmployeeOnboardingField> EmployeeOnboardingField { get; set; } // many-to-many EmployeeOnboardingField.FK_EmployeeOnboardingField_OnboardingField;

    }
}
