using System.Collections.Generic;
using System.Linq;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.Enums;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantJobSiteQuery : Query<ApplicantJobSite, IApplicantJobSiteQuery>, IApplicantJobSiteQuery
    {
        #region Constructor

        public ApplicantJobSiteQuery(IEnumerable<ApplicantJobSite> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        #endregion

        IApplicantJobSiteQuery IApplicantJobSiteQuery.OrderByDescription()
        {
            OrderBy(x => x.JobSiteDescription);
            return this;
        }

        IApplicantJobSiteQuery IApplicantJobSiteQuery.ByApplicantJobSiteId(ApplicantJobSiteEnum applicantJobSiteId)
        {
            FilterBy(x => x.ApplicantJobSiteId == applicantJobSiteId);
            return this;
        }
    }
}