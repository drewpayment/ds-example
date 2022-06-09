using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantSchoolTypeQuery : IQuery<ApplicantSchoolType, IApplicantSchoolTypeQuery>
    {
        IApplicantSchoolTypeQuery BySchoolTypeId(int schoolTypeId);
        IApplicantSchoolTypeQuery OrderByApplicantSchoolTypeId();
        IApplicantSchoolTypeQuery OrderByDescription();
        IApplicantSchoolTypeQuery OrderByApplicationOrder();
    }
}