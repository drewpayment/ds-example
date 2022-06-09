using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IPaycheckSUTAHistoryQuery : IQuery<PaycheckSUTAHistory, IPaycheckSUTAHistoryQuery>
    {
        /// <summary>
        /// Filters history for a single client.
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        IPaycheckSUTAHistoryQuery ByClientId(int clientId);

        /// <summary>
        /// Gets Paycheck Pay Data by PayrollID
        /// </summary>
        /// <param name="payrollId"></param>
        /// <returns></returns>
        IPaycheckSUTAHistoryQuery ByPayrollId(int payrollId);

        /// <summary>
        /// Gets Paycheck Pay Data by PayrollID
        /// </summary>
        /// <param name="payrollId"></param>
        /// <returns></returns>
        IPaycheckSUTAHistoryQuery ByGenPaycheckPayDataHistoryID(int? genPaycheckPayDataHistoryID);

        /// <summary>
        /// Gets Paycheck Pay Data between 2 date ranges
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IPaycheckSUTAHistoryQuery ByDateRange(DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// Filters history for a single employee.
        /// </summary>
        /// <param name="employeeId">ID of employee to filter by.</param>
        /// <returns></returns>
        IPaycheckSUTAHistoryQuery ByEmployeeId(int employeeId);
    }
}
