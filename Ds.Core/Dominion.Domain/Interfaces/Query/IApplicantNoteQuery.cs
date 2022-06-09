using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantNoteQuery : IQuery<ApplicantNote, IApplicantNoteQuery>
    {
        IApplicantNoteQuery ByApplicantId(int applicantId);
        IApplicantNoteQuery ByUserId(int userId);
        IApplicantNoteQuery ByApplicantNoteId(int remarkId);
        IApplicantNoteQuery ByClientId(int clientId);
        IApplicantNoteQuery ByApplicantPostingId(int applicantPostingId);
    }
}