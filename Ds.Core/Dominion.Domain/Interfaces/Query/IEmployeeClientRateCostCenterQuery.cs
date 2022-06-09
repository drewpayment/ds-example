using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeClientRateCostCenterQuery : IQuery<EmployeeClientRateCostCenter, IEmployeeClientRateCostCenterQuery>
    {

        /// <summary>
        /// Filters by rates for a given employee
        /// </summary>
        /// <param name="employeeId">ID of employee to filter by.</param>
        /// <returns></returns>
        IEmployeeClientRateCostCenterQuery ByEmployeeId(params int[] employeeId);

        /// <summary>
        /// Filters Employee Rates by a single ClientRateId
        /// </summary>
        /// <param name="clientRateId">ID of client to filter by.</param>
        /// <returns></returns>
        IEmployeeClientRateCostCenterQuery ByClientRateId(int clientRateId);

        /// <summary>
        /// Filters by rates for a given client
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        IEmployeeClientRateCostCenterQuery ByClientId(int clientId);

        /// <summary>
        /// Filters by rates for a given client
        /// </summary>
        /// <param name="clientIds">Array of IDs of clients to filter by.</param>
        /// <returns></returns>
        IEmployeeClientRateCostCenterQuery ByClientIds(int[] clientIds);

        /// <summary>
        /// Filters by rates associated with a given cost center.
        /// </summary>
        /// <param name="costCenterId"></param>
        /// <returns></returns>
        IEmployeeClientRateCostCenterQuery ByCostCenter(int costCenterId);
    }
}
