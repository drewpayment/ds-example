using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.LaborManagement.Dto.JobCosting;
using Dominion.Utility.OpResult;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientJobCostingQuery : IQuery<ClientJobCosting, IClientJobCostingQuery>, IByClientIdQuery<IClientJobCostingQuery>
    {
        /// <summary>
        /// Filters the job costs by whether they are enabled.
        /// </summary>
        /// <param name="isEnabled">Whether the job costs should be enabled. Defaults to true.</param>
        /// <returns></returns>
        IClientJobCostingQuery ByIsEnabled(bool isEnabled = true);
        IClientJobCostingQuery OrderBySequence();
        IClientJobCostingQuery ByClientJobCostingId(int clientJobCostingId);
    }
}
