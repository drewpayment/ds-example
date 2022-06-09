using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeClientRateQuery : IQuery<EmployeeClientRate, IEmployeeClientRateQuery>
    {
        /// <summary>
        /// Filters by rates for a given client
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        IEmployeeClientRateQuery ByClientId(int clientId);

        /// <summary>
        /// Filters by rates for a given employee
        /// </summary>
        /// <param name="employeeId">ID of employee to filter by.</param>
        /// <returns></returns>
        IEmployeeClientRateQuery ByEmployeeId(params int[] employeeId);

        /// <summary>
        /// Filters by rates an employees default rate status
        /// </summary>
        /// <param name="isDefault">If true (default), will only return default rates.</param>
        /// <returns></returns>
        IEmployeeClientRateQuery ByIsDefault(bool isDefault = true);

        /// <summary>
        /// Filters by rates for a given client rate
        /// </summary>
        /// <param name="clientRateId">ID of client to filter by.</param>
        /// <returns></returns>
        IEmployeeClientRateQuery ByClientRateId(int clientRateId);
        IEmployeeClientRateQuery ByEmployeeClientRate(int employeeClientRateId);
    }
}
