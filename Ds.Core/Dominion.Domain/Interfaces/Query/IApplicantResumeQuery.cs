using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantResumeQuery : IQuery<ApplicantResume, IApplicantResumeQuery>
    {
        IApplicantResumeQuery ByApplicantId(int applicantId);
        IApplicantResumeQuery ByApplicantResumeId(int applicantResumeId);
    }
}