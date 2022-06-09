using System.Collections.Generic;
using System.Linq;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantJobTypeQuery : Query<ApplicantJobType, IApplicantJobTypeQuery>, IApplicantJobTypeQuery
    {
        #region Constructor

        public ApplicantJobTypeQuery(IEnumerable<ApplicantJobType> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        #endregion

        IApplicantJobTypeQuery IApplicantJobTypeQuery.OrderByApplicantJobTypeId()
        {
            OrderBy(x => x.ApplicantJobTypeId);
            return this;
        }

        IApplicantJobTypeQuery IApplicantJobTypeQuery.OrderByDescription()
        {
            OrderBy(x => x.Description);
            return this;
        }
    }
}