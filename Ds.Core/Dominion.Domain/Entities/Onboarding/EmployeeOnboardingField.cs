using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Onboarding
{
    public  class EmployeeOnboardingField : Entity<EmployeeOnboardingField>
    {
      public virtual int EmployeeId { get; set; } 
		public virtual int OnboardingFieldId { get; set; } 
		public virtual string StringValue { get; set; } 
		public virtual bool? IsBoolValue { get; set; } 
		public virtual int? IntValue { get; set; } 
		public virtual decimal? DecimalValue { get; set; } 


		//FOREIGN KEYS
		public virtual OnboardingField OnboardingField { get; set; } 
        public virtual EmployeeOnboarding EmployeeOnboarding { get; set; } 
       

    }
}
