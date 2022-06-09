using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantEducationHistoryQuery : IQuery<ApplicantEducationHistory, IApplicantEducationHistoryQuery>
    {
        IApplicantEducationHistoryQuery ByApplicantId(int applicantId);
        IApplicantEducationHistoryQuery ByApplicantEducationId(int applicantEducationId);
        IApplicantEducationHistoryQuery ByIsActive(bool flag);
    }
}