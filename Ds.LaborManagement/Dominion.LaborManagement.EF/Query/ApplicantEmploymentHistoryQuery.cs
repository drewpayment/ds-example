using System.Collections.Generic;
using System.Linq;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantEmploymentHistoryQuery : Query<ApplicantEmploymentHistory, IApplicantEmploymentHistoryQuery>, IApplicantEmploymentHistoryQuery
    {
        public ApplicantEmploymentHistoryQuery(IEnumerable<ApplicantEmploymentHistory> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        IApplicantEmploymentHistoryQuery IApplicantEmploymentHistoryQuery.ByApplicantEmploymentHistoryId(int applicantEmploymentHistoryId)
        {
            FilterBy(x => x.ApplicantEmploymentId == applicantEmploymentHistoryId);
            return this;
        }

        IApplicantEmploymentHistoryQuery IApplicantEmploymentHistoryQuery.ByApplicantId(int applicantId)
        {
            FilterBy(x => x.ApplicantId == applicantId);
            return this;
        }

        IApplicantEmploymentHistoryQuery IApplicantEmploymentHistoryQuery.ByIsActive(bool flag)
        {
            FilterBy(x => x.IsEnabled == flag);
            return this;
        }

        public IApplicantEmploymentHistoryQuery NotIn(IEnumerable<int> applicantEmploymentHistoryIds)
        {
            FilterBy(x => !applicantEmploymentHistoryIds.Contains(x.ApplicantEmploymentId));
            return this;
        }
    }
}