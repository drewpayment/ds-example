using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IPaycheckPayDataHistoryQuery : IQuery<PaycheckPayDataHistory, IPaycheckPayDataHistoryQuery>
    {
        /// <summary>
        /// Filters by paycheck history belonging to the specified client group.
        /// </summary>
        /// <param name="clientGroupId">Group to filter by.</param>
        /// <returns></returns>
        IPaycheckPayDataHistoryQuery ByClientGroupId(int? clientGroupId);

        /// <summary>
        /// Filters by paycheck history belonging to the specified client cost center.
        /// </summary>
        /// <param name="costCenterId"></param>
        /// <returns></returns>
        IPaycheckPayDataHistoryQuery ByClientCostCenterId(int? costCenterId);

        /// <summary>
        /// Filters history for a single client.
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        IPaycheckPayDataHistoryQuery ByClientId(int clientId);
        IPaycheckPayDataHistoryQuery ByYear(int year);
        IPaycheckPayDataHistoryQuery ByDateRange(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Filters by paycheck history id
        /// </summary>
        /// <param name="paycheckHistoryId"></param>
        /// <returns></returns>
        IPaycheckPayDataHistoryQuery ByPaycheckHistoryId(int paycheckHistoryId);

        IPaycheckPayDataHistoryQuery ByPaycheckHistoryIds(IEnumerable<int> paycheckPayDataHistoryIds);

        /// <summary>
        /// Gets Paycheck Pay Data by PayrollID
        /// </summary>
        /// <param name="payrollId"></param>
        /// <returns></returns>
        IPaycheckPayDataHistoryQuery ByPayrollId(int payrollId);
    }
}
