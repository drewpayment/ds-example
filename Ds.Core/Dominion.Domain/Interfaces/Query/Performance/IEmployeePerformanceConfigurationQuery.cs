using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface IEmployeePerformanceConfigurationQuery : IQuery<EmployeePerformanceConfiguration, IEmployeePerformanceConfigurationQuery>
    {
        /// <summary>
        /// Filters employee configurations by a single employee.
        /// </summary>
        /// <returns></returns>
        IEmployeePerformanceConfigurationQuery ByEmployee(int employeeId);
    }
}