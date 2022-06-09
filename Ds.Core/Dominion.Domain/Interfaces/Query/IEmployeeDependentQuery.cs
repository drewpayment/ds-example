using System;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Queries <see cref="EmployeeDependent"/> data.
    /// </summary>
    public interface IEmployeeDependentQuery : IQuery<EmployeeDependent, IEmployeeDependentQuery>
    {
        IEmployeeDependentQuery ByEmployeeDependentId(int employeeDependentId);

        /// <summary>
        /// Filters dependents by those who belong to the specified employee.
        /// </summary>
        /// <param name="employeeId">Employee to filter by.</param>
        /// <returns></returns>
        IEmployeeDependentQuery ByEmployeeId(int employeeId);

        /// <summary>
        /// Filters dependents by those who belong to one of the specified employees.
        /// </summary>
        /// <param name="employeeIds">Employees to filter by.</param>
        /// <returns></returns>
        IEmployeeDependentQuery ByEmployeeIds(params int[] employeeIds);

        /// <summary>
        /// Filters by dependents that have been approved by the company admin.
        /// </summary>
        /// <returns></returns>
        IEmployeeDependentQuery ByApprovedDependentsOnly();

        IEmployeeDependentQuery IncludeInActiveDependents(bool includeInActive);

        /// <summary>
        /// Filters by dependents that do not have 1095-C covered individual data for the specified year.
        /// </summary>
        /// <param name="year">Year.</param>
        /// <returns></returns>
        IEmployeeDependentQuery ByDoesNotHave1095CCoverageInYear(int year);

        IEmployeeDependentQuery ByInactive(bool isInactive, int? calendarYear);
    }
}
