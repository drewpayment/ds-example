using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantReferenceQuery : Query<ApplicantReference, IApplicantReferenceQuery>, IApplicantReferenceQuery
    {
        public ApplicantReferenceQuery(IEnumerable<ApplicantReference> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        IApplicantReferenceQuery IApplicantReferenceQuery.ByApplicantReferenceId(int applicantReferenceId)
        {
            FilterBy(x => x.ApplicantReferenceId == applicantReferenceId);
            return this;
        }

        IApplicantReferenceQuery IApplicantReferenceQuery.ByApplicantId(int applicantId)
        {
            FilterBy(x => x.ApplicantId == applicantId);
            return this;
        }

        IApplicantReferenceQuery IApplicantReferenceQuery.IsEnabled(bool isEnabled)
        {
            FilterBy(x => x.IsEnabled == isEnabled);
            return this;
        }

    }
}

