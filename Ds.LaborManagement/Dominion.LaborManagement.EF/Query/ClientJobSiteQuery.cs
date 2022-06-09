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
    public class ClientJobSiteQuery : Query<ClientJobSite, IClientJobSiteQuery>, IClientJobSiteQuery
    {
        #region Constructor

        public ClientJobSiteQuery(IEnumerable<ClientJobSite> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        #endregion

        IClientJobSiteQuery IClientJobSiteQuery.ByClientJobSiteId(int clientJobSiteId)
        {
            FilterBy(x => x.ClientJobSiteId == clientJobSiteId);
            return this;
        }

        IClientJobSiteQuery IClientJobSiteQuery.ByApplicantJobSiteId(ApplicantJobSiteEnum applicantJobSiteId)
        {
            FilterBy(x => x.ApplicantJobSiteId == applicantJobSiteId);
            return this;
        }
        IClientJobSiteQuery IClientJobSiteQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId == clientId);
            return this;
        }
        IClientJobSiteQuery IClientJobSiteQuery.BySharePosts(bool canShare)
        {
            FilterBy(x => x.SharePosts == canShare);
            return this;
        }
    }
}