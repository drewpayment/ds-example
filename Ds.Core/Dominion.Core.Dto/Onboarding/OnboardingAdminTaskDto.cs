using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Onboarding
{
    public class OnboardingAdminTaskDto
    {
        public int OnboardingAdminTaskId { get; set; }
        public int OnboardingAdminTaskListId { get; set; }
        public string Description { get; set; }
        public string OldDescription { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsEditing { get; set; }
    }
}
