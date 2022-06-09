using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantCompanyCorrespondenceQuery : IQuery<ApplicantCompanyCorrespondence, IApplicantCompanyCorrespondenceQuery>
    {
        IApplicantCompanyCorrespondenceQuery ByClientId(int clientId);
        IApplicantCompanyCorrespondenceQuery ByCorrespondenceTypeId(LaborManagement.Dto.ApplicantTracking.ApplicantCorrespondenceType? correspondenceTypeId);
        IApplicantCompanyCorrespondenceQuery ByIsActive(bool isActive);
        IApplicantCompanyCorrespondenceQuery ByIsText(bool isText);
        IApplicantCompanyCorrespondenceQuery OrderByApplicantCompanyCorrespondenceId();
        IApplicantCompanyCorrespondenceQuery OrderByDescription();
        IApplicantCompanyCorrespondenceQuery ByCorrespondenceId(int correspondenceId);
    }
}