using System;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public partial class ApplicantOnBoardingDefaultDetailDto
    {
        public int ApplicantOnBoardingDefaultDetailId { get; set; }
        public int ClientId { get; set; }
        public int ApplicantPostingId { get; set; }
        public int ApplicantOnBoardingProcessId { get; set; }
        public int ApplicantOnBoardingTaskId { get; set; }
        public int AssignedToUserId { get; set; }
        public bool IsEmailRequired { get; set; }
        public int DaysToComplete { get; set; }
        public string SpecialInstructions { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsAutoStart { get; set; }
    }
}