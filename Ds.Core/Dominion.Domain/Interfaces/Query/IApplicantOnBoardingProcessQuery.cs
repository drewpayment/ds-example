using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantOnBoardingProcessQuery : IQuery<ApplicantOnBoardingProcess, IApplicantOnBoardingProcessQuery>
    {
        IApplicantOnBoardingProcessQuery ByClientId(int clientId);
        IApplicantOnBoardingProcessQuery ByCustomToPostingId(int customToPostingId);
        IApplicantOnBoardingProcessQuery ByApplicantOnboardingProcessId(int applicantOnboardingProcessId);
        IApplicantOnBoardingProcessQuery ByIsEnabled(bool isEnabled);
        IApplicantOnBoardingProcessQuery OrderByApplicantOnBoardingProcessId();
        IApplicantOnBoardingProcessQuery OrderByDescription();
    }
}