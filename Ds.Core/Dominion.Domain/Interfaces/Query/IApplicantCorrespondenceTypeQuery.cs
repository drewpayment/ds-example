using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantCorrespondenceTypeQuery : IQuery<ApplicantCorrespondenceTypeInfo, IApplicantCorrespondenceTypeQuery>
    {
        IApplicantCorrespondenceTypeQuery OrderByApplicantCorrespondenceTypeId();
        IApplicantCorrespondenceTypeQuery OrderByDescription();
    }
}