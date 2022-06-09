using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Entities.Forms;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Onboarding
{
    public class EmployeeOnboardingWorkflow : Entity<EmployeeOnboardingWorkflow>, IHasModifiedData
    {
        public virtual int                    EmployeeId               { get; set; }
        public virtual int                    OnboardingWorkflowTaskId { get; set; }
        public virtual bool?                  IsCompleted              { get; set; }
        public virtual int?                   FormDefinitionId         { get; set; }
        public virtual int                    ModifiedBy               { get; set; }
        public virtual DateTime               Modified                 { get; set; }
        public virtual string                 SignatureDescription     { get; set; }
        public virtual int?                   FormTypeId               { get; set; }
        public virtual bool                   IsDeleted                { get; set; }

        //Related Entities
        public virtual FormDefinition          FormDefinitions         { get; set; }
        public virtual ICollection<Resource>   Resources               { get; set; }
        public virtual OnboardingWorkflowTask  OnboardingWorkflowTask  { get; set; }
        
        public virtual EmployeeOnboarding      EmployeeOnboarding      { get; set; }
        public virtual Employee.Employee       Employee                { get; set;  }
		public virtual FormType                FormType 			   { get; set; }
    }
}
