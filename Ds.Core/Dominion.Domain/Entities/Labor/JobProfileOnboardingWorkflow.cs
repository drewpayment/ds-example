using System.Collections.Generic;
using Dominion.Core.Dto.EEOC;
using Dominion.Core.Dto.Employee;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.EEOC;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Forms;
using Dominion.Domain.Entities.Onboarding;

namespace Dominion.Domain.Entities.Labor
{
    public partial class JobProfileOnboardingWorkflow : Entity<JobProfileOnboardingWorkflow>
    {
        public virtual int JobProfileId { get; set; }
        public virtual int OnboardingWorkflowTaskId { get; set; }
        public virtual int? FormTypeId { get; set; }
        public virtual JobProfile JobProfile { get; set; }
        public virtual OnboardingWorkflowTask OnboardingWorkflowTask { get; set; }
    }
}

