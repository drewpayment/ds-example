using System;
using System.Collections.Generic;
using Dominion.LaborManagement.Dto.ApplicantTracking;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public partial class ApplicantOnBoardingTaskDto
    {
        public int ApplicantOnboardingTaskId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public int DefaultDaysToComplete { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public int DefaultAssignedToUserId { get; set; }
        public bool IsDefaultIsEmailRequired { get; set; }
        public string DefaultSpecialInstructions { get; set; }
        public int ProcessPhaseId { get; set; }
        public int AttachmentCount { get; set; }
        public ApplicantStatusType ApplicantStatusTypeId { get; set; }
        public List<ApplicantOnboardingTaskAttachmentDto> ApplicantOnboardingTaskAttachment { get; set; }
    }
}