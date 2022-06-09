using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IGenW2EmployeeHistoryQuery : IQuery<GenW2EmployeeHistory, IGenW2EmployeeHistoryQuery>
    {
        /// <summary>
        /// Filters by ClientID, with a parameter to include all clients in an organization.
        /// </summary>
        /// <param name="clientId">Filters by ClientID</param>
        /// <param name="includeAllClientsInOrganization">Allows filtering by all clients in organization, defaulting to true.</param>
        /// <returns></returns>
        IGenW2EmployeeHistoryQuery ByClientId(int clientId, bool includeAllClientsInOrganization = true);
        /// <summary>
        /// Filters by W2 Year
        /// </summary>
        /// <param name="w2Year">W2 year to get information for.</param>
        /// <returns></returns>
        IGenW2EmployeeHistoryQuery ByW2Year(int w2Year);
        /// <summary>
        /// Filters by EmployeeID
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        IGenW2EmployeeHistoryQuery ByEmployeeId(int employeeId);
    }
}
