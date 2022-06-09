using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Onboarding
{
    public class EmployeeOnboardingWorkflowEmpFlowDto
    {
        public EmployeeOnboardingWorkflowDto WorkflowEmp {get; set; }
        public OnboardingWorkflowDto WorkflowClient { get; set; }
    }
}
