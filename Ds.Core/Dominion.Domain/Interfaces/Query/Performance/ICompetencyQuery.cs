using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface ICompetencyQuery : IQuery<Competency, ICompetencyQuery>
    {
        /// <summary>
        /// Filters entities by performance competency id.
        /// </summary>
        /// <param name="competencyId"></param>
        /// <returns></returns>
        ICompetencyQuery ByCompetencyId(int competencyId);

        /// <summary>
        /// Filters entities by client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        ICompetencyQuery ByClient(int clientId);

        /// <summary>
        /// Filters entities by their active status.
        /// </summary>
        /// <param name="isArchived"></param>
        /// <returns></returns>
        ICompetencyQuery ByArchived(bool isArchived = false);

        /// <summary>
        /// Filters entities to default, system performance competencies.
        /// </summary>
        /// <returns></returns>
        ICompetencyQuery ByDefault();

        /// <summary>
        /// Filters a list of entities by a list of performance competency ids. 
        /// </summary>
        /// <param name="competencyIdList"></param>
        /// <returns></returns>
        ICompetencyQuery ByCompetencyIdList(List<int> competencyIdList);

        /// <summary>
        /// Filters by comptencies with the specified 'Core' status. 
        /// </summary>
        /// <param name="isCore">Defaults to true.</param>
        /// <returns></returns>
        ICompetencyQuery ByIsCore(bool isCore = true);

        ICompetencyQuery ByEvaluation(int evaluationId);
    }
}
