using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantApplicationEmailHistoryQuery : IQuery<ApplicantApplicationEmailHistory, IApplicantApplicationEmailHistoryQuery>
    {
        IApplicantApplicationEmailHistoryQuery ByCompanyCorrespondenceId(int? correspondenceId);
        IApplicantApplicationEmailHistoryQuery ByApplicationHeaderId(int applicationHeaderId);
        IApplicantApplicationEmailHistoryQuery ByEmailHistoryId(int applicationEmailHistoryId);
        IApplicantApplicationEmailHistoryQuery BySenderId(int userId);
    }
}