using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface ICompetencyGroupQuery : IQuery<CompetencyGroup, ICompetencyGroupQuery>
    {
        /// <summary>
        /// Filters entities by client id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        ICompetencyGroupQuery ByClient(int clientId);

        /// <summary>
        /// Filters entities by competency group id. 
        /// </summary>
        /// <param name="competencyGroupId"></param>
        /// <returns></returns>
        ICompetencyGroupQuery ByCompetencyGroup(int competencyGroupId);
    }
}
