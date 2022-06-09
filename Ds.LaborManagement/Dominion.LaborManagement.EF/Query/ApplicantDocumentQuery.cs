using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantDocumentQuery : Query<ApplicantDocument, IApplicantDocumentQuery>, IApplicantDocumentQuery
    {
        public ApplicantDocumentQuery(IEnumerable<ApplicantDocument> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        IApplicantDocumentQuery IApplicantDocumentQuery.ByApplicantId(int applicantId)
        {
            FilterBy(x => x.ApplicantApplicationHeader.ApplicantId == applicantId);
            return this;
        }

        IApplicantDocumentQuery IApplicantDocumentQuery.ByApplicationHeaderId(int applicationHeaderId)
        {
            FilterBy(x => x.ApplicationHeaderId == applicationHeaderId);
            return this;
        }

        IApplicantDocumentQuery IApplicantDocumentQuery.ByApplicantDocumentId(int applicantDocumentId)
        {
            FilterBy(x => x.ApplicantDocumentId == applicantDocumentId);
            return this;
        }
    }
}