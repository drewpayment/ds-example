using Dominion.Aca.Dto.Forms;
using Dominion.Domain.Entities.Aca;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Query on <see cref="Aca1095CCoveredIndividual"/>(s).
    /// </summary>
    public interface IAca1095CCoveredIndividualQuery : IQuery<Aca1095CCoveredIndividual, IAca1095CCoveredIndividualQuery>
    {
        /// <summary>
        /// Filters by covered individuals for the given employee.
        /// </summary>
        /// <param name="employeeId">ID of the employee.</param>
        /// <returns></returns>
        IAca1095CCoveredIndividualQuery ByEmployeeId(int employeeId);

        /// <summary>
        /// Filters by covered individuals belonging to any of the provided employees.
        /// </summary>
        /// <param name="employeeIds">ID(s) of employees to filter by.</param>
        /// <returns></returns>
        IAca1095CCoveredIndividualQuery ByEmployeeIds(params int[] employeeIds);

        /// <summary>
        /// Filters covered individuals for the given year.
        /// </summary>
        /// <param name="year">1095C reporting year.</param>
        /// <returns></returns>
        IAca1095CCoveredIndividualQuery ByYear(int year);

        /// <summary>
        /// Filters covered individuals for the given client.
        /// </summary>
        /// <param name="clientId">ID of the client.</param>
        /// <returns></returns>
        IAca1095CCoveredIndividualQuery ByClient(int clientId);

        /// <summary>
        /// Filters by the type of covered individual (e.g. Employee or Employee Dependent).
        /// </summary>
        /// <param name="types">Covered individual type(s) to fitler by.</param>
        /// <returns></returns>
        IAca1095CCoveredIndividualQuery ByCoveredIndividualType(params Aca1095CCoveredIndividualType[] types);

        IAca1095CCoveredIndividualQuery ByEmployeeDependentActive(bool isActive);
    }
}
