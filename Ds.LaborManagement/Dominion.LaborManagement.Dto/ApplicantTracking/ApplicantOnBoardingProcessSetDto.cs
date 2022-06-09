using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    [Serializable]
    public class ApplicantOnBoardingProcessSetDto
    {
        public int ApplicantOnboardingProcessId { get; set; }
        public int ApplicantOnboardingTaskId { get; set; }
        public string Description { get; set; }
        public int OrderId { get; set; }
        public bool IsSelected { get; set; } = false;
        public ApplicantOnBoardingTaskDto ApplicantOnBoardingTask { get; set; }
    }
}
