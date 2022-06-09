using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public partial class ApplicantOnBoardingProcessDto
    {
        public int ApplicantOnboardingProcessId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public int? CustomToPostingId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public int ApplicantOnBoardingProcessTypeId { get; set; }
        public int ProcessPhaseId { get; set; }

        public List<ApplicantOnBoardingProcessSetDto> ApplicantOnBoardingProcessSets { get; set; }
    }
}