using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantCompanyApplicationQuery : IQuery<ApplicantCompanyApplication, IApplicantCompanyApplicationQuery>
    {
        IApplicantCompanyApplicationQuery ByClientId(int clientId);
        IApplicantCompanyApplicationQuery ByApplicationId(int applicationId);
        IApplicantCompanyApplicationQuery ByIsCurrentEmpApp(bool isCurrentEmpApp);
        IApplicantCompanyApplicationQuery ByIsActive(bool isActive);
        IApplicantCompanyApplicationQuery OrderByCompanyApplicationId();
        IApplicantCompanyApplicationQuery OrderByDescription();
    }
}