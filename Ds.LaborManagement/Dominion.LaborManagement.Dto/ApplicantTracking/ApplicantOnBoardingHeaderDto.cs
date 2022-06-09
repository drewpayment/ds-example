using System;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantOnBoardingHeaderDto
    {
        public int ApplicantOnBoardingHeaderId { get; set; }
        public int ApplicantApplicationHeaderId { get; set; }
        public int ApplicantOnBoardingProcessId { get; set; }
        public int ApplicantId { get; set; }
        public int ClientId { get; set; }
        public DateTime? OnBoardingStarted { get; set; }
        public DateTime? OnBoardingEnded { get; set; }
        public bool IsHired { get; set; }
        public bool IsRejected { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
    }
}