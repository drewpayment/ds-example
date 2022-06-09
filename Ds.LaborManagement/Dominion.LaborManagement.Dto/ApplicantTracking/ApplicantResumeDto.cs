using System;
namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantResumeDto
    {
        public int ApplicantResumeId { get; set; }
        public int ApplicantId { get; set; }
        public string LinkLocation { get; set; }
        public DateTime DateAdded { get; set; }
    }
}