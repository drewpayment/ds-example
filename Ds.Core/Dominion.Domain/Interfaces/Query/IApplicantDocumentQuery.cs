using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantDocumentQuery : IQuery<ApplicantDocument, IApplicantDocumentQuery>
    {
        IApplicantDocumentQuery ByApplicantId(int applicantId);
        IApplicantDocumentQuery ByApplicationHeaderId(int applicationHeaderId);
        IApplicantDocumentQuery ByApplicantDocumentId(int applicantDocumentId);
    }
}