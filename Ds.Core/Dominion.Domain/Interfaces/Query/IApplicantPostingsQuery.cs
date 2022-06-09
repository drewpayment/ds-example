using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Dto.Enums;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantPostingsQuery : IQuery<ApplicantPosting, IApplicantPostingsQuery>
    {
        IApplicantPostingsQuery ByClientId(int clientId);
        IApplicantPostingsQuery ByPostingId(int postingId);
        IApplicantPostingsQuery ByApplicantApplicationHeaderId(int applicantApplicationHeaderId);
        IApplicantPostingsQuery ByExternallyViewable();
        IApplicantPostingsQuery ShowFilledPostings();
        IApplicantPostingsQuery ShowOpenPostings();
        IApplicantPostingsQuery ByIsActive(bool flag);
        IApplicantPostingsQuery ByIsClosed(bool flag);
        IApplicantPostingsQuery ByHasNoCompletedApplications();
        IApplicantPostingsQuery WhereFilledDateNotNull();
        IApplicantPostingsQuery ByNoOfDays(DateTime startDate, int days);
        IApplicantPostingsQuery ByIncludeGeneralApplication(bool flag);
        IApplicantPostingsQuery ByWithinPublishedDates();
        IApplicantPostingsQuery ByPublishDateRange(DateTime startDate, DateTime endDate);
        IQueryResult<GetNewApplicantsDateByDateSpanInnerJoinResultDto> QueryNewApplicantDataPoints(
            IApplicantApplicationHeaderQuery headerQuery,
            IApplicantPostingsQuery postingsQuery, int clientId, DateTime startDate, DateTime endDate);

        IQueryResult<IEnumerable<ApplicantDaysToHireDetailDto>> QueryApplicantDaysToHireDetail(
            IApplicantPostingCategoriesQuery postingCategoriesQuery, IApplicantPostingsQuery postingsQuery,
            int clientId);
        /// <summary>
        /// Retuns the postings for clients who have the provided <see cref="ApplicantJobSiteEnum"/> and have <see cref="ClientJobSite.SharePosts"/> marked as true
        /// </summary>
        /// <param name="jobsite">The <see cref="ApplicantJobSite"/> to include <see cref="ApplicantPosting"/>'s for.</param>
        /// <returns></returns>
        IApplicantPostingsQuery ForClientsWithApplicantJobSite(ApplicantJobSiteEnum jobsite);
        IApplicantPostingsQuery ByApplicationId(int applicationId);
    }
}