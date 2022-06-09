using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Forms;
using Dominion.Domain.Interfaces.Entities;
using System.Collections.Generic;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;

namespace Dominion.Domain.Entities.Onboarding
{
    public partial class OnboardingAdminTaskList : Entity<OnboardingAdminTaskList>, IHasModifiedData
    {
        public virtual int OnboardingAdminTaskListId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }

        public virtual Client Client { get; set; }
        public virtual ICollection<JobProfile> JobProfiles { get; set; }
        public virtual ICollection<OnboardingAdminTask> OnboardingAdminTask { get; set; }
    }
}
