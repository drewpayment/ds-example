using System;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantRejectionReasonDto
    {
        public int ApplicantRejectionReasonId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
    }
}