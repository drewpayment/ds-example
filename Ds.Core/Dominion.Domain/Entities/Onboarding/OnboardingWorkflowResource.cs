using Dominion.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Onboarding
{
    public class OnboardingWorkflowResource : Entity<OnboardingWorkflowResource>
    {
        public virtual int OnboardingWorkflowTaskId { get; set; }
        public virtual int CompanyResourceId { get; set; }
        public virtual OnboardingWorkflowTask OnboardingWorkflowTask { get; set; }
        public virtual Resource Resource { get; set; }
    }
}
