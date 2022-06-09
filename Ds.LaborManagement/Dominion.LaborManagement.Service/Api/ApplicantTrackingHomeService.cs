using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Authentication.Intermediate.Util;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.Utility.OpResult;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.Service.Api
{

    /// <summary>
    /// This service ties authorization and business logic together.
    /// </summary>
    public class ApplicantTrackingHomeService : IApplicantTrackingHomeService
    {
        private readonly IApplicantTrackingHomeProvider _provider;
        private readonly IApplicantTrackingAuthProvider _authProvider;


        public ApplicantTrackingHomeService(IApplicantTrackingHomeProvider provider, IApplicantTrackingAuthProvider authProvider)
        {
            _provider = provider;
            _authProvider = authProvider;
        }

        public IOpResult<IEnumerable<NewApplicantDataPointDto>> GetNewApplicantsDataPointsByDateSpan(int clientId, DateTime startDate, DateTime endDate)
        {
            return _authProvider.AuthorizeByClientIdFn(() => _provider.GetNewApplicantsDataPointsByDateSpanFn(clientId, startDate, endDate), clientId);
        }

        public IOpResult<AverageDaysToFillDto> GetAverageDaysToFillPositions(int clientId)
        {
            return _authProvider.AuthorizeByClientIdFn(() => _provider.GetAverageDaysToFillPositionsFn(clientId), clientId);
        }

        public IOpResult<IEnumerable<IEnumerable<ApplicantDaysToHireDetailDto>>> GetAverageDaysToFillPositionsDetail(int clientId)
        {
            return _authProvider.AuthorizeByClientIdFn(() => _provider.GetAverageDaysToFillPositionsDetailFn(clientId), clientId);
        }

        public IOpResult<IEnumerable<PostingApplicationsDetailDto>> GetPostingApplicationsDetail(int clientId)
        {
            return _authProvider.AuthorizeByClientIdFn(() => _provider.GetPostingApplicationsDetailFn(clientId), clientId);
        }

        public IOpResult<TurnoverRateDto> GetEmployeeTurnoverRate(int clientId)
        {
            return _authProvider.AuthorizeByClientIdFn(() => _provider.GetEmployeeTurnoverRateFn(clientId), clientId);
        }

        public IOpResult<IEnumerable<EmployeePayAndEmployeeStatusAndEmployee>> GetTerminatedEmployeesDetail(int clientId)
        {
            return _authProvider.AuthorizeByClientIdFn(() => _provider.GetTerminatedEmployeesDetailFn(clientId), clientId);
        }

        public IOpResult<IEnumerable<RejectionReasonsDto>> GetApplicantRejectionReasonCounts(int clientId)
        {
            return _authProvider.AuthorizeByClientIdFn(() => _provider.GetApplicantRejectionReasonCountsFn(clientId), clientId);
        }

        public IOpResult<PostsWithoutApplicantsDto> GetTheNumberOfPostsWithoutApplicants(int clientId)
        {
            return _authProvider.AuthorizeByClientIdFn(() => _provider.GetTheNumberOfPostsWithoutApplicantsFn(clientId), clientId);
        }

        public IOpResult<ApplicantsToReviewDto> GetTheNumberApplicantsToReview(int clientId)
        {
            return _authProvider.AuthorizeByClientIdFn(() => _provider.GetTheNumberApplicantsToReviewFn(clientId), clientId);
        }

        public IOpResult<CandidateCountDto> GetTheTotalNumberOfCandidates(int clientId)
        {
            return _authProvider.AuthorizeByClientIdFn(() => _provider.GetTheTotalNumberOfCandidatesFn(clientId), clientId);
        }

        public IOpResult<ApplicantTrackingHomePageDto> GetAllDataForHomePage(int clientId, DateTime startDate, DateTime endDate)
        {
            return _authProvider.AuthorizeByClientIdFn(() => _provider.GetAllDataForHomePageFn(clientId, startDate, endDate), clientId);
        }

        public IOpResult<OpenPostsToFillDto> GetTheNumberOfOpenPosts(int clientId)
        {
            return _authProvider.AuthorizeByClientIdFn(() => _provider.GetTheNumberOfOpenPosts(clientId), clientId);
        }
    }
}
