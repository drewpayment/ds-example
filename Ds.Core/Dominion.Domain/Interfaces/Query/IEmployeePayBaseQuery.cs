using System.Collections.Generic;
using Dominion.Core.Dto.Employee;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Provides a way to query <see cref="EmployeePay"/> entities.
    /// </summary>
    public interface IEmployeePayBaseQuery<TEmployeePay, TQuery> : IQuery<TEmployeePay, TQuery> 
        where TEmployeePay : class, IEmployeePay
        where TQuery : class, IEmployeePayBaseQuery<TEmployeePay, TQuery> 
    {
        /// <summary>
        /// Filters employees by the entity's EmployeeStatus.IsActive status.
        /// </summary>
        /// <param name="isActive">Indication if the employee's status is marked as active.</param>
        /// <returns></returns>
        TQuery ByEmployeeActivity(bool isActive);

        /// <summary>
        /// Filters employees by the entity's employment status.
        /// </summary>
        /// <param name="status">Employment status to filter by (eg: FullTime, PartTime, etc)</param>
        /// <returns></returns>
        TQuery ByEmploymentStatus(EmployeeStatusType status);

        /// <summary>
        /// Filters employees by the cost center they belong to.
        /// </summary>
        /// <param name="clientCostCenterId">ID of the cost center to filter by. If null, will filter by employees that
        /// have not been assigned to a cost center.</param>
        /// <returns></returns>
        TQuery ByCostCenter(int? clientCostCenterId);

        /// <summary>
        /// Filters employees by the division they belong to.
        /// </summary>
        /// <param name="clientDivisionId">ID of the division to filter by. If null, will filter by employees that
        /// have not been assigned to a division.</param>
        /// <returns></returns>
        TQuery ByDivision(int? clientDivisionId);

        /// <summary>
        /// Filters employees by the department they belong to.
        /// </summary>
        /// <param name="clientDepartmentId">ID of the department to filter by. If null, will filter by employees that
        /// have not been assigned to a department.</param>
        /// <returns></returns>
        TQuery ByDepartment(int? clientDepartmentId);

        /// <summary>
        /// Filters employees whose first OR last name contain the given text.
        /// </summary>
        /// <param name="name">Name text to filter the first and/or last name by.</param>
        /// <returns></returns>
        TQuery ByEmployeeName(string name);

        /// <summary>
        /// Filters employees whose employee number contains the given number.
        /// </summary>
        /// <param name="number">Number to filter the employee number by.</param>
        /// <returns></returns>
        TQuery ByEmployeeNumber(string number);

        /// <summary>
        /// Filters employees who belong to the specified client account.
        /// </summary>
        /// <param name="clientId">ID of the client to filter employee pay info by.</param>
        /// <returns></returns>
        TQuery ByClient(int clientId);

        /// <summary>
        /// Filters employees by employee id.
        /// </summary>
        /// <param name="employeeId">ID of the employee to filter by.</param>
        /// <returns></returns>
        TQuery ByEmployeeId(int employeeId);

        /// <summary>
        /// Filters employees by is terminated.
        /// </summary>
        /// <returns></returns>
        TQuery ByIsTerminated();

        /// <summary>
        /// Filters employees by those whose employee id is in the provided collection.
        /// </summary>
        /// <param name="employeeIds">IDs of the employees to filter by.</param>
        /// <returns></returns>
        TQuery ByEmployeeIds(IEnumerable<int> employeeIds);
        TQuery ByType(int Type);

        /// <summary>
        /// Filters employees by those who have one of the given employment statues.
        /// </summary>
        /// <param name="statuses">The list of employment statuses to filter by.</param>
        /// <returns></returns>
        TQuery ByEmploymentStatuses(IEnumerable<EmployeeStatusType> statuses);

        /// <summary>
        /// Filters employees by those who DO NOT have one of the given employment statues.
        /// </summary>
        /// <param name="excludedStatuses">The list of employment statues to exclude.</param>
        /// <returns></returns>
        TQuery ByExcludeEmploymentStatuses(IEnumerable<EmployeeStatusType> excludedStatuses);

        /// <summary>
        /// Filters employees by the entity's EmployeeStatus.1099Exempt status.
        /// </summary>
        /// <param name="is1099Exempt">Indication if the employee is marked as 1099 exempt.</param>
        /// <returns></returns>
        TQuery ByIs1099Exempt(bool is1099Exempt = true);

        /// <summary>
        /// Filters employees by those whose time policy id is in the provided collection.
        /// </summary>
        /// <param name="timePolicyIds">IDs of the time policies to filter by.</param>
        /// <returns></returns>
        TQuery ByTimePolicyIds(IEnumerable<int> timePolicyIds);
    }
}