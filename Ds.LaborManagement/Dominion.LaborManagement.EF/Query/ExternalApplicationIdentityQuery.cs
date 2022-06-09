using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class ExternalApplicationIdentityQuery : Query<ExternalApplicationIdentity, IExternalApplicationIdentityQuery>, IExternalApplicationIdentityQuery
    {
        public IExternalApplicationIdentityQuery ByExternalApplicationId(string id)
        {
            FilterBy(x => id.Equals(x.ExternalApplicationId, StringComparison.Ordinal));
            return this;
        }

        public ExternalApplicationIdentityQuery(IEnumerable<ExternalApplicationIdentity> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }
    }
}
