using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Employee
{
    public interface IEmployeeCostCenterPercentageQuery : IQuery<EmployeeCostCenterPercentage, IEmployeeCostCenterPercentageQuery>
    {
        /// <summary>
        /// Filters by cost center percentages related to a particular client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IEmployeeCostCenterPercentageQuery ByClientId(int clientId);

        /// <summary>
        /// Filters by cost center percentages related to a particular cost center;
        /// </summary>
        /// <param name="costCenterId"></param>
        /// <returns></returns>
        IEmployeeCostCenterPercentageQuery ByCostCenterId(int costCenterId);
        IEmployeeCostCenterPercentageQuery ByEmployeeId(int employeeId);
    }
}
