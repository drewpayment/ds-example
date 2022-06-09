using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantCoverLetterRequiredQuery : IQuery<ApplicantCoverLetterRequired, IApplicantCoverLetterRequiredQuery>
    {
        IApplicantCoverLetterRequiredQuery OrderByApplicantCoverLetterRequiredId();
        IApplicantCoverLetterRequiredQuery OrderByDescription();
    }
}