using Dominion.Utility.Query;
using Dominion.Domain.Entities.Aca;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Queries <see cref="AcaEmployeeApproval"/> data.
    /// </summary>
    public interface IAcaEmployeeApprovalQuery : IQuery<AcaEmployeeApproval, IAcaEmployeeApprovalQuery>
    {
        /// <summary>
        /// Filters Employee approval by Client and Year.
        /// </summary>
        /// <param name="clientId">Employee to filter by.</param>
        /// <param name="year">Employee to filter by.</param>
        /// <returns>Query to be further filtered.</returns>
        IAcaEmployeeApprovalQuery ByClientIdYear(int clientId, int year);

        /// <summary>
        /// Filters by approvals within the specified ACA reporting year.
        /// </summary>
        /// <param name="year">ACA reporting year to filter by.</param>
        /// <returns>Query to be further filtered.</returns>
        IAcaEmployeeApprovalQuery ByYear(int year);

        /// <summary>
        /// Filters by approvals for the specified client.
        /// </summary>
        /// <param name="clientId">ID of the client to filter by.</param>
        /// <returns>Query to be further filtered.</returns>
        IAcaEmployeeApprovalQuery ByClient(int clientId);

        /// <summary>
        /// Filters by approvals for the specified employee.
        /// </summary>
        /// <param name="employeeId">ID of the employee to filter by.</param>
        /// <returns>Query to be further filtered.</returns>
        IAcaEmployeeApprovalQuery ByEmployee(int employeeId);

        /// <summary>
        /// Filters by indicated approval status.
        /// </summary>
        /// <param name="isApproved">If true, will filter by only approvals with a non-null <see cref="AcaEmployeeApproval.ApprovalDate"/>.</param>
        /// <returns>Query to be further filtered.</returns>
        IAcaEmployeeApprovalQuery ByIsApproved(bool isApproved);
    }
}
