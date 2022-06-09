using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicationViewedQuery : Query<ApplicationViewed, IApplicationViewedQuery>, IApplicationViewedQuery
    {
        public ApplicationViewedQuery(IEnumerable<ApplicationViewed> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        IApplicationViewedQuery IApplicationViewedQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId == clientId);
            return this;
        }

        IApplicationViewedQuery IApplicationViewedQuery.ByUserId(int userId)
        {
            FilterBy(x => x.UserId == userId);
            return this;
        }

        IApplicationViewedQuery IApplicationViewedQuery.ByApplicationViewedId(int applicationViewedId)
        {
            FilterBy(x => x.ApplicationViewedId == applicationViewedId);
            return this;
        }

        IApplicationViewedQuery IApplicationViewedQuery.ByApplicationHeaderId(int applicationHeaderId)
        {
            FilterBy(x => x.ApplicationHeaderId == applicationHeaderId);
            return this;
        }

    }
}
