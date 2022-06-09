using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantStatusTypeQuery : IQuery<ApplicantStatusTypeDetail, IApplicantStatusTypeQuery>
    {
        IApplicantStatusTypeQuery ByStatusTypeId(LaborManagement.Dto.ApplicantTracking.ApplicantStatusType statusTypeId);
        IApplicantStatusTypeQuery ByIsActive(bool isActive);
    }
}