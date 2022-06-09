using System;
using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Onboarding
{
    /// <summary>
    /// Entity representation of a [dbo].[EmployeeOnboarding] record.
    /// </summary>

    public partial class EmployeeOnboarding : Entity<EmployeeOnboarding>, IHasModifiedData
    {
		public virtual int EmployeeId { get; set; } 
		public virtual int ClientId { get; set; } 
		public virtual DateTime OnboardingInitiated { get; set; } 
		public virtual DateTime? EmployeeStarted { get; set; } 
		public virtual DateTime? EmployeeSignoff { get; set; } 
		public virtual DateTime? OnboardingEnd { get; set; }
        public virtual DateTime? ESSActivated { get; set; }
        public virtual DateTime? InvitationSent { get; set; }
        public virtual bool? UserAddedDuringOnboarding { get; set; }
        public virtual int ModifiedBy { get; set; } 
		public virtual DateTime Modified { get; set; } 
 		
		//FOREIGN KEYS
		public virtual Client            Client   { get; set; }
		public virtual Employee.Employee Employee { get; set; }
        public virtual ICollection<EmployeeOnboardingTasks> Tasks { get; set; } // many-to-one;

        public virtual ICollection<EmployeeOnboardingField> EmployeeOnboardingField { get; set; } // many-to-many EmployeeOnboardingField.FK_EmployeeOnboardingField_EmployeeOnboarding;

        public virtual ICollection<EmployeeOnboardingWorkflow> EmployeeWorkflow { get; set; }
 

        public virtual ICollection<EmployeeOnboardingForm> OnboardingForms { get; set; } // many-to-many EmployeeOnboardingForm.FK_EmployeeOnboardingForm_EmployeeOnboarding;
	}
    
}