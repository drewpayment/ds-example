using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantResumeQuery : Query<ApplicantResume, IApplicantResumeQuery>, IApplicantResumeQuery
    {
        public ApplicantResumeQuery(IEnumerable<ApplicantResume> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        IApplicantResumeQuery IApplicantResumeQuery.ByApplicantId(int applicantId)
        {
            FilterBy(x => x.ApplicantId == applicantId);
            return this;
        }

        IApplicantResumeQuery IApplicantResumeQuery.ByApplicantResumeId(int applicantResumeId)
        {
            FilterBy(x => x.ApplicantResumeId == applicantResumeId);
            return this;
        }
    }
}