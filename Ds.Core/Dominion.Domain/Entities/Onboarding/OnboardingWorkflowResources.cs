using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Forms;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Core;

namespace Dominion.Domain.Entities.Onboarding
{
    public class OnboardingWorkflowResources : Entity<OnboardingWorkflowResources>
    {
        public virtual int OnboardingWorkflowTaskId { get; set; }
        public virtual int CompanyResourceId { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual OnboardingWorkflowTask OnboardingWorkflowTask { get; set; }
        public virtual Resource Resource { get; set; }
    }
}
