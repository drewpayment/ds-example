using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Client;

namespace Dominion.Core.Dto.Onboarding
{
    public class EmployeeOnboardingWorkflowDto
    {
        public virtual int                       EmployeeId                { get; set; }
        public virtual int                       OnboardingWorkflowTaskId  { get; set; }
        public virtual bool?                     IsCompleted               { get; set; }
        public virtual bool?                     IsHeader                  { get; set; }
        public virtual int?                      FormTypeId                { get; set; }
        public virtual int?                      FormDefinitionId          { get; set; }
        public virtual int                       ModifiedBy                { get; set; }
        public virtual DateTime                  Modified                  { get; set; } 
        public virtual int?                      StateId                   { get; set; }
        public virtual string                    FormDefinition            { get; set; }
        public virtual IEnumerable<CompanyResourceDto> Resources                { get; set; }
                                                                           
        public virtual OnboardingWorkflowTaskDto OnboardingTask              { get; set; }
        public virtual bool                      IsDeleted                  { get; set; }
    }
}
