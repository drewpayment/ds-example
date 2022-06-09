using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantNoteQuery : Query<ApplicantNote, IApplicantNoteQuery>, IApplicantNoteQuery
    {
        public ApplicantNoteQuery(IEnumerable<ApplicantNote> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        IApplicantNoteQuery IApplicantNoteQuery.ByApplicantId(int applicantId)
        {
            FilterBy(x => x.ApplicantId == applicantId);
            return this;
        }

        IApplicantNoteQuery IApplicantNoteQuery.ByApplicantNoteId(int applicantNoteId)
        {
            FilterBy(x => x.Remark.RemarkId == applicantNoteId);
            return this;
        }

        IApplicantNoteQuery IApplicantNoteQuery.ByUserId(int userId)
        {
            FilterBy(x => x.Remark.AddedBy == userId);
            return this;
        }

        IApplicantNoteQuery IApplicantNoteQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.Remark.User.UserClientSettings.Select(y => y.ClientId).Contains(clientId));
            //FilterBy(x => x.Applicant.ApplicantApplicationHeaders.Select(y => y.ApplicantPosting.ClientId).Contains(clientId);
            //FilterBy(x => x.Remark.User.LastClientId == clientId);
            return this;
        }

        IApplicantNoteQuery IApplicantNoteQuery.ByApplicantPostingId(int applicantPostingId)
        {
            FilterBy(x => x.Applicant.ApplicantApplicationHeaders
                .Select(y => y.ApplicantPosting.PostingId)
                .Where(y => y == applicantPostingId)
                .Contains(applicantPostingId)
            );
            return this;
        }
    }
}