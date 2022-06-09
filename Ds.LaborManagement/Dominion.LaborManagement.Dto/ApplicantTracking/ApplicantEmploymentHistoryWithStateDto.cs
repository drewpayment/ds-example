using Dominion.Core.Dto.Location;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantEmploymentHistoryWithStateDto : ApplicantEmploymentHistoryDto
    {
        public StateDto State { get; set; }
    }
}
