using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface IPerformanceCompetencyQuery : IQuery<PerformanceCompetency, IPerformanceCompetencyQuery>
    {
        /// <summary>
        /// Filters entities by performance competency id.
        /// </summary>
        /// <param name="performanceCompetencyId"></param>
        /// <returns></returns>
        IPerformanceCompetencyQuery ByPerformanceCompetencyId(int performanceCompetencyId);

        /// <summary>
        /// Filters entities by client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IPerformanceCompetencyQuery ByClient(int clientId);

        /// <summary>
        /// Filters entities by their active status.
        /// </summary>
        /// <param name="inactive"></param>
        /// <returns></returns>
        IPerformanceCompetencyQuery ByActive(bool inactive = false);

        /// <summary>
        /// Filters entities to default, system performance competencies.
        /// </summary>
        /// <returns></returns>
        IPerformanceCompetencyQuery ByDefault();

        /// <summary>
        /// Filters a list of entities by a list of performance competency ids. 
        /// </summary>
        /// <param name="competencyIdList"></param>
        /// <returns></returns>
        IPerformanceCompetencyQuery ByPerformanceCompetencyIdList(List<int> competencyIdList);
    }
}
