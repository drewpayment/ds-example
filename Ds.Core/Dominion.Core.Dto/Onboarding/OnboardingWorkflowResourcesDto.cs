using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Onboarding
{
    public class OnboardingWorkflowResourcesDto
    {
        public int OnboardingWorkflowTaskId { get; set; }
        public int CompanyResourceId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
