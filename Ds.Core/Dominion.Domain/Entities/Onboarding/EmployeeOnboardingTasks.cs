using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Onboarding
{
    public class EmployeeOnboardingTasks : Entity<EmployeeOnboardingTasks>, IHasModifiedData

    {
        public virtual int EmployeeOnboardingTaskId { get; set; } 
        public virtual int EmployeeId { get; set; } 
        public virtual int Sequence { get; set; }
        public virtual int ModifiedBy { get; set; } 
        public virtual DateTime Modified { get; set; }

        public virtual int TaskId { get; set; } // Core.Task.TaskId

        //Foreign Keys
        public virtual EmployeeOnboarding EmployeeOnboarding { get; set; }

        public virtual Task Task { get; set; }
    }
    
}