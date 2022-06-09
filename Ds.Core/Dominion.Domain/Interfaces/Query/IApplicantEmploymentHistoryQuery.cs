using System.Collections.Generic;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantEmploymentHistoryQuery : IQuery<ApplicantEmploymentHistory, IApplicantEmploymentHistoryQuery>
    {
        IApplicantEmploymentHistoryQuery ByApplicantEmploymentHistoryId(int applicantEmploymentHistoryId);
        IApplicantEmploymentHistoryQuery ByApplicantId(int applicantId);
        IApplicantEmploymentHistoryQuery ByIsActive(bool flag);
        IApplicantEmploymentHistoryQuery NotIn(IEnumerable<int> applicantEmploymentHistoryIds);
    }
}
