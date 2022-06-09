using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Onboarding
{
    public class OnboardingAdminTask : Entity<OnboardingAdminTask>, IHasModifiedData
    {
        public virtual int OnboardingAdminTaskId { get; set; }
        public virtual int OnboardingAdminTaskListId { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }

        public virtual OnboardingAdminTaskList OnboardingAdminTaskList { get; set; }
    }
}
