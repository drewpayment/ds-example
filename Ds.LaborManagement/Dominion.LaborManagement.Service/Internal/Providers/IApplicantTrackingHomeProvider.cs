using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Employee;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    public interface IApplicantTrackingHomeProvider
    {
        IOpResult<IEnumerable<NewApplicantDataPointDto>> GetNewApplicantsDataPointsByDateSpanFn(int clientId, DateTime startDate, DateTime endDate);
        IOpResult<AverageDaysToFillDto> GetAverageDaysToFillPositionsFn(int clientId);
        IOpResult<IEnumerable<IEnumerable<ApplicantDaysToHireDetailDto>>> GetAverageDaysToFillPositionsDetailFn(int clientId);
        IOpResult<IEnumerable<PostingApplicationsDetailDto>> GetPostingApplicationsDetailFn(int clientId);
        IOpResult<TurnoverRateDto> GetEmployeeTurnoverRateFn(int clientId);
        IOpResult<IEnumerable<EmployeePayAndEmployeeStatusAndEmployee>> GetTerminatedEmployeesDetailFn(int clientId);
        IOpResult<IEnumerable<EmployeePayAndEmployeeStatusAndEmployee>> GetActiveEmployeesDetailFn(int clientId, DateTime startDate, DateTime endDate);
        IOpResult<IEnumerable<RejectionReasonsDto>> GetApplicantRejectionReasonCountsFn(int clientId);
        IOpResult<PostsWithoutApplicantsDto> GetTheNumberOfPostsWithoutApplicantsFn(int clientId);
        IOpResult<ApplicantsToReviewDto> GetTheNumberApplicantsToReviewFn(int clientId);
        IOpResult<CandidateCountDto> GetTheTotalNumberOfCandidatesFn(int clientId);
        IOpResult<ApplicantTrackingHomePageDto> GetAllDataForHomePageFn(int clientId, DateTime startDate, DateTime endDate);
        IOpResult<OpenPostsToFillDto> GetTheNumberOfOpenPosts(int clientId);
    }
}
