using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Onboarding
{
     [Serializable]
    public class OnboardingWorkflow : Entity<OnboardingWorkflow>, IHasModifiedData
    {
        public virtual int OnboardingWorkflowId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual DateTime Modified { get; set; }
        
    }
}
