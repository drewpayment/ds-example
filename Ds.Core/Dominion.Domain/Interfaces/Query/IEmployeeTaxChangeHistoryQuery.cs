using System;
using System.Collections;
using System.Collections.Generic;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Tax;
using Dominion.Taxes.Dto;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Provides a way to query <see cref="EmployeePay"/> entities.
    /// </summary>
    public interface IEmployeeTaxChangeHistoryQuery : IQuery<EmployeeTaxChangeHistory, IEmployeeTaxChangeHistoryQuery>
    {
        /// <summary>
        /// Filters change history by employee id.
        /// </summary>
        /// <param name="active">Defaults to true; send in false if you prefer otherwise.</param>
        /// <returns></returns>
        IEmployeeTaxChangeHistoryQuery ByIsActive(bool active = true);

        /// <summary>
        /// Filters change history by employee id.
        /// </summary>
        /// <param name="employeeId">ID of the employee to filter by.</param>
        /// <returns></returns>
        IEmployeeTaxChangeHistoryQuery ByEmployeeId(int employeeId);

        /// <summary>
        /// Filters change history by employee ids.
        /// </summary>
        /// <param name="employeeIds">IDs of the employees to filter by.</param>
        /// <returns></returns>
        IEmployeeTaxChangeHistoryQuery ByEmployeeIds(IEnumerable<int> employeeIds);

        /// <summary>
        /// Filters the change history by date; less than or equal to.
        /// Filtering by 'less than' allows us to look at previous payroll data.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        IEmployeeTaxChangeHistoryQuery ByChangeDateLTE(DateTime date);

        /// <summary>
        /// Gets the records that represents past payrolls.
        /// </summary>
        /// <param name="lteDate">Date of the payroll.</param>
        /// <param name="employeeIds"></param>
        /// <returns></returns>
        IQueryResult<EmployeeTaxChangeHistory> PastPayroll(DateTime lteDate, IEnumerable<int> employeeIds);

        /// <summary>
        /// Orders the query results by <see cref="EmployeeTaxChangeHistory.ChangeId"/>.
        /// </summary>
        /// <param name="direction">Defaults to ascending.</param>
        /// <returns></returns>
        IEmployeeTaxChangeHistoryQuery OrderByChangeId(SortDirection direction = SortDirection.Ascending);

    }
}