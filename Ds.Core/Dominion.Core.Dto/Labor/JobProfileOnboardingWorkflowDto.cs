using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public class JobProfileOnboardingWorkflowDto
    {
        public int JobProfileId { get; set; }
        public int OnboardingWorkflowTaskId { get; set; }
        public int? FormTypeId { get; set; }
        public int? OnboardingAdminTaskListId { get; set; }
        public bool? isRequired { get; set; }
    }
}
