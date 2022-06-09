using Dominion.Core.Dto.Labor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Onboarding
{
    public class OnboardingAdminTaskListDto
    {
        public int OnboardingAdminTaskListId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsSelected { get; set; }
        public List<OnboardingAdminTaskDto> OnboardingAdminTasks { get; set; }
        public List<JobProfileBasicDto> JobProfiles { get; set; }
    }
}
