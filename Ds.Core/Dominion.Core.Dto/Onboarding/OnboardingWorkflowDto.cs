using System;

namespace Dominion.Core.Dto.Onboarding
{
    public class OnboardingWorkflowDto
    {
        public virtual int      OnboardingWorkflowId { get; set; }
        public virtual int      ClientId         { get; set; }
        public virtual int      ModifiedBy       { get; set; }
        public virtual DateTime Modified         { get; set; }
        public virtual OnboardingWorkflowTaskDto EmployeeOnboardingWorkflowDto { get; set; }

    }
}
