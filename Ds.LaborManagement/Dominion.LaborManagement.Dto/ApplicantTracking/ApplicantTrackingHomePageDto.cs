using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Employee;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantTrackingHomePageDto
    {
        public IEnumerable<NewApplicantDataPointDto> NewApplicantDataPoints { get; set; }
        public AverageDaysToFillDto AverageDaysToFillDto { get; set; }
        public IEnumerable<IEnumerable<ApplicantDaysToHireDetailDto>> AverageDaysToFillDetailDto { get; set; }
        public TurnoverRateDto TurnoverRateDto { get; set; }
        public IEnumerable<EmployeePayAndEmployeeStatusAndEmployee> TerminatedEmployees { get; set; }
        public IEnumerable<RejectionReasonsDto> RejectionReasonDtos { get; set; }
        public PostsWithoutApplicantsDto PostsWithoutApplicantsDto { get; set; }
        public ApplicantsToReviewDto ApplicantsToReviewDto { get; set; }

        public CandidateCountDto CandidateCountDto { get; set; }
        public OpenPostsToFillDto OpenPostsToFillDto { get; set; }
    }
}
