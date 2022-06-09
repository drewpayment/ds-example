using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantDegreeListQuery : Query<ApplicantDegreeList, IApplicantDegreeListQuery>, IApplicantDegreeListQuery
    {
        public ApplicantDegreeListQuery(IEnumerable<ApplicantDegreeList> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        IApplicantDegreeListQuery IApplicantDegreeListQuery.ByDegreeId(int degreeId)
        {
            FilterBy(x => x.DegreeId == degreeId);
            return this;
        }
    }
}