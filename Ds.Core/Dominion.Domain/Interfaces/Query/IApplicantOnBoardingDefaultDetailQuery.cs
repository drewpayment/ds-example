using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantOnBoardingDefaultDetailQuery : IQuery<ApplicantOnBoardingDefaultDetail, IApplicantOnBoardingDefaultDetailQuery>
    {
        IApplicantOnBoardingDefaultDetailQuery ByApplicantOnBoardingDefaultDetailId(int applicantOnBoardingDefaultDetailId);
        IApplicantOnBoardingDefaultDetailQuery ByApplicantOnBoardingProcessId(int applicantOnBoardingProcessId);
        IApplicantOnBoardingDefaultDetailQuery ByApplicantPostingId(int applicantPostingId);
    }
}