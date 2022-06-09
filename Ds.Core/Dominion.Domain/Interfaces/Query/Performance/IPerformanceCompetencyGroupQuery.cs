using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface IPerformanceCompetencyGroupQuery : IQuery<CompetencyGroup, IPerformanceCompetencyGroupQuery>
    {
        /// <summary>
        /// Filters entities by client id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IPerformanceCompetencyGroupQuery ByClient(int clientId);

        /// <summary>
        /// Filters entities by competency group id. 
        /// </summary>
        /// <param name="competencyGroupId"></param>
        /// <returns></returns>
        IPerformanceCompetencyGroupQuery ByCompetencyGroup(int competencyGroupId);
    }
}
