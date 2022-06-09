using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Onboarding
{
    public  class EmployeeOnboardingMenuClient : Entity<EmployeeOnboardingMenuClient>, IHasModifiedData
    {
        public virtual int      ClientId   { get; set; }
        public virtual byte     SeqNo      { get; set; }
        public virtual byte     MenuId     { get; set; }
        public virtual int      ModifiedBy { get; set; }
        public virtual DateTime Modified   { get; set; }

        //FOREIGN KEYS
        public virtual EmployeeOnboardingMenuItems EmployeeOnboardingMenuItems { get; set; } 
    }
}
