using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantDegreeListQuery : IQuery<ApplicantDegreeList, IApplicantDegreeListQuery>
    {
        IApplicantDegreeListQuery ByDegreeId(int degreeId);
    }
}
