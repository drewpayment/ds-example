using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantOnBoardingHeaderQuery : IQuery<ApplicantOnBoardingHeader, IApplicantOnBoardingHeaderQuery>
    {
        IApplicantOnBoardingHeaderQuery ByApplicantOnBoardingHeaderId(int applicantOnBoardingHeaderId);
        IApplicantOnBoardingHeaderQuery ByApplicantApplicationHeaderId(int applicantApplicationHeaderId);
        IApplicantOnBoardingHeaderQuery ByApplicantId(int applicantId);
        IApplicantOnBoardingHeaderQuery ByApplicantOnBoardingProcessId(int applicantOnBoardingProcessId);
    }
}