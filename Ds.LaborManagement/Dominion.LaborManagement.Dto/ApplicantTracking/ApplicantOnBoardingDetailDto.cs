using System;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public partial class ApplicantOnBoardingDetailDto
    {
        public int ApplicantOnBoardingDetailId { get; set; }
        public int ApplicantOnBoardingHeaderId { get; set; }
        public int ApplicantOnBoardingTaskId { get; set; }
        public int AssignedToUserId { get; set; }
        public bool IsEmailRequired { get; set; }
        public int DaysToComplete { get; set; }
        public string SpecialInstructions { get; set; }
        public DateTime? DateStarted { get; set; }
        public DateTime? DateCompleted { get; set; }
        public bool IsCompleted { get; set; }
        public int ModifiedBy { get; set; }
        public int ApplicantCorrespondenceId { get; set; }
        public DateTime? Modified { get; set; }
    }
}