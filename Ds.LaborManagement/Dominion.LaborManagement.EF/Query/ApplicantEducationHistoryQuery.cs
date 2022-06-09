using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantEducationHistoryQuery : Query<ApplicantEducationHistory, IApplicantEducationHistoryQuery>, IApplicantEducationHistoryQuery
    {
        public ApplicantEducationHistoryQuery(IEnumerable<ApplicantEducationHistory> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        IApplicantEducationHistoryQuery IApplicantEducationHistoryQuery.ByApplicantId(int applicantId)
        {
            FilterBy(x => x.ApplicantId == applicantId);
            return this;
        }

        IApplicantEducationHistoryQuery IApplicantEducationHistoryQuery.ByApplicantEducationId(int applicantEducationId)
        {
            FilterBy(x => x.ApplicantEducationId == applicantEducationId);
            return this;
        }

        IApplicantEducationHistoryQuery IApplicantEducationHistoryQuery.ByIsActive(bool flag)
        {
            FilterBy(x => x.IsEnabled == flag);
            return this;
        }
    }
}