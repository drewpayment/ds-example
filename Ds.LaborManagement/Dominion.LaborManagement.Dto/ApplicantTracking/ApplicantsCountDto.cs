using System;
namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantsCountDto
    {
        public ApplicantStatusType StatusId { get; set; }
        public int Count   { get; set; }
    }
}