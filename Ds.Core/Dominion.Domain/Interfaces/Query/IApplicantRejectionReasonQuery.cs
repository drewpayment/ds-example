using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantRejectionReasonQuery : IQuery<ApplicantRejectionReason, IApplicantRejectionReasonQuery>
    {
        IApplicantRejectionReasonQuery ByApplicantRejectionReasonId(int rejectionReasonId);
        IApplicantRejectionReasonQuery ByClientId(int clientId);
        IApplicantRejectionReasonQuery ByIsActive(bool isActive);
        IApplicantRejectionReasonQuery OrderByDescription();
        IApplicantRejectionReasonQuery ByDescription(string descriptionText);

        IQueryResult<RejectionReasonsDto> JoinApplicantApplicationHeaderAndApplicantRejectionReason(
            IApplicantRejectionReasonQuery rejectionQuery, IApplicantApplicationHeaderQuery headerQuery);
    }
}