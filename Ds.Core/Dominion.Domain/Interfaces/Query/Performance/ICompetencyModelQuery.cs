using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface ICompetencyModelQuery : IQuery<CompetencyModel, ICompetencyModelQuery>
    {
        /// <summary>
        /// Filters competency models by a single client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        ICompetencyModelQuery ByClient(int clientId);

        /// <summary>
        /// Filters competency models by a single model.
        /// </summary>
        /// <param name="competencyModelId"></param>
        /// <returns></returns>
        ICompetencyModelQuery ByCompetencyModelId(int competencyModelId);

        /// <summary>
        /// Filters models by employee.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        ICompetencyModelQuery ByEmployee(int employeeId);
    }
}
