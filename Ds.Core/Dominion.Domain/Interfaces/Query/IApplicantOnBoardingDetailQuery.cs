using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantOnBoardingDetailQuery : IQuery<ApplicantOnBoardingDetail, IApplicantOnBoardingDetailQuery>
    {
        IApplicantOnBoardingDetailQuery ByApplicantOnBoardingDetailId(int applicantOnBoardingDetailId);
        IApplicantOnBoardingDetailQuery ByApplicantOnBoardingHeaderId(int applicantOnBoardingHeaderId);
    }
}