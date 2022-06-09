using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query
{
    public interface ITaxDefermentHistoryQuery : IQuery<TaxDefermentHistory, ITaxDefermentHistoryQuery>
    {
        /// <summary>
        /// Filters history for a single client.
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        ITaxDefermentHistoryQuery ByClientId(int clientId);

        /// <summary>
        /// Gets Paycheck Tax Deferment Data by PayrollID
        /// </summary>
        /// <param name="payrollId"></param>
        /// <returns></returns>
        ITaxDefermentHistoryQuery ByPayrollId(int payrollId);

        /// <summary>
        /// Gets Paycheck Tax Deferment Data between 2 date ranges
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        ITaxDefermentHistoryQuery ByDateRange(DateTime? startDate, DateTime? endDate);
    }
}
