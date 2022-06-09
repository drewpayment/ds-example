using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using System.Linq;
using System.Text;

namespace Dominion.LaborManagement.EF.Query
{
    public class ClientJobCostingAssignmentSelectedQuery :
        Query<ClientJobCostingAssignmentSelected, IClientJobCostingAssignmentSelectedQuery>,
        IClientJobCostingAssignmentSelectedQuery
    {
        public ClientJobCostingAssignmentSelectedQuery(IEnumerable<ClientJobCostingAssignmentSelected> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        IClientJobCostingAssignmentSelectedQuery IClientJobCostingAssignmentSelectedQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId.Equals(clientId));
            return this;
        }

        IClientJobCostingAssignmentSelectedQuery IClientJobCostingAssignmentSelectedQuery.ByIsEnabled(bool isEnabled)
        {
            FilterBy(x => x.IsEnabled.Equals(isEnabled));
            return this;
        }

        IClientJobCostingAssignmentSelectedQuery IClientJobCostingAssignmentSelectedQuery.BySelectedJobCostings(int[] parentJobCostingIds)
        {
            FilterBy(x => x.ClientJobCostingId_Selected != null && parentJobCostingIds.Contains(x.ClientJobCostingId_Selected ?? 0));
            return this;
        }
    }
}