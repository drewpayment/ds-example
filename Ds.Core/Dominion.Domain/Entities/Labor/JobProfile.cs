using System.Collections.Generic;
using Dominion.Domain.Entities.Api;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;
using System;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Domain.Entities.Onboarding;

namespace Dominion.Domain.Entities.Labor
{
    /// <summary>
    /// EF entity representing a [dbo].[JobProfile] record.
    /// </summary>
    public partial class JobProfile : Entity<JobProfile>, IHasModifiedData
    {
        public virtual int    JobProfileId   { get; set; } 
        public virtual int    ClientId     { get; set; } 
        public virtual string Description  { get; set; } 
        public virtual string Code         { get; set; } 
        public virtual string Requirements { get; set; } 
        public virtual bool   IsActive     { get; set; }
        public string WorkingConditions { get; set; }
        public string Benefits { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int? CompetencyModelId { get; set; }
        public virtual int? OnboardingAdminTaskListId { get; set; }
        public virtual Client Client { get; set; }
        public virtual CompetencyModel CompetencyModel { get; set; }
        public virtual OnboardingAdminTaskList OnboardingAdminTaskList { get; set; }
        public virtual ICollection<Employee.Employee> Employees { get; set; }

        public virtual JobProfileClassifications JobProfileClassifications { get; set; }

        public virtual JobProfileCompensation JobProfileCompensation { get; set; }
		public virtual ICollection<ApiAccountMapping> ApiAccountMapping { get; set; } // many-to-one;

        public virtual ICollection<JobProfileResponsibilities> JobProfileResponsibilities { get; set; }
        public virtual ICollection<JobProfileSkills> JobProfileSkills { get; set; }
        public virtual ICollection<JobProfileAccruals> JobProfileAccruals { get; set; }
        public virtual ICollection<JobProfileOnboardingWorkflow> JobProfileOnboardingWorkflows { get; set; }

    }
}
