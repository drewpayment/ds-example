using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class ClientJobCostingQuery : Query<ClientJobCosting, IClientJobCostingQuery>, IClientJobCostingQuery
    {
        public ClientJobCostingQuery(IEnumerable<ClientJobCosting> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        public IClientJobCostingQuery ByClientId(int clientId)
        {
            this.FilterBy(x => x.ClientId == clientId);
            return this;
        }

        public IClientJobCostingQuery ByIsEnabled(bool isEnabled = true)
        {
            this.FilterBy(x => x.IsEnabled == isEnabled);
            return this;
        }

        public IClientJobCostingQuery OrderBySequence()
        {
            this.OrderBy(x => x.Sequence);
            return this;
        }

        public IClientJobCostingQuery ByClientJobCostingId(int clientJobCostingId)
        {
            this.FilterBy(x => x.ClientJobCostingId == clientJobCostingId);
            return this;
        }
    }

    public class ClientJobCostingAssignmentQuery : Query<ClientJobCostingAssignment, IClientJobCostingAssignmentQuery>, IClientJobCostingAssignmentQuery
    {
        public ClientJobCostingAssignmentQuery(IEnumerable<ClientJobCostingAssignment> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        IClientJobCostingAssignmentQuery IClientJobCostingAssignmentQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId.Equals(clientId));
            return this;
        }

        IClientJobCostingAssignmentQuery IClientJobCostingAssignmentQuery.ByIsEnabled(bool isEnabled)
        {
            FilterBy(x => x.IsEnabled.Equals(isEnabled));
            return this;
        }

        IClientJobCostingAssignmentQuery IClientJobCostingAssignmentQuery.ByClientJobCostingAssignmentId(int clientJobCostingAssignmentId)
        {
            FilterBy(x => x.ClientJobCostingAssignmentId.Equals(clientJobCostingAssignmentId));
            return this;
        }

        IClientJobCostingAssignmentQuery IClientJobCostingAssignmentQuery.ByClientJobCostingParents(
            int[] parentJobCostingIds)
        {
            FilterBy(x => parentJobCostingIds.Contains(x.ClientJobCostingId));
            return this;
        }

    }
}
