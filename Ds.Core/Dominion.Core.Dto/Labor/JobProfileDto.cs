using Dominion.Core.Dto.LeaveManagement;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Labor
{
    public class JobProfileDto
    {
        public int    JobProfileId { get; set; }
        public int    ClientId     { get; set; }
        public string Description  { get; set; }
        public string Code         { get; set; }
        public string Requirements { get; set; }
        public bool   IsActive     { get; set; }
        public string WorkingConditions { get; set; }
        public string Benefits { get; set; }
        public bool IsBenefitPortalOn { get; set; }
        public bool IsApplicantTrackingOn { get; set; }
        public string SourceURL { get; set; }
        public int? CompetencyModelId { get; set; }
        public string CompetencyModelName { get; set; }
        public bool IsOnboardingEnabled { get; set; }
        public bool? IsPerformanceReviewsEnabled { get; set; }
        public int? OnboardingAdminTaskListId { get; set; }

        public JobProfileClassificationsDto Classifications { get; set; }

        public JobProfileCompensationDto Compensation { get; set; }
        public IList<JobResponsibilitiesDto> JobResponsibilities { get; set; }
 	    public IList<JobSkillsDto> JobSkills { get; set; }
		public IEnumerable<JobProfileAccrualsDto> JobProfileAccruals { get; set; }
        public IEnumerable<JobProfileOnboardingWorkflowDto> JobProfileOnboardingWorkflows { get; set; }
    }
}
