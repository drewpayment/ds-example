using System;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IPayrollHistoryQuery : IQuery<PayrollHistory, IPayrollHistoryQuery>
    {
        /// <summary>
        /// Filters the payroll results by the unique Payroll ID.
        /// </summary>
        /// <param name="payrollId">Payroll ID to filter the payroll results by.</param>
        /// <returns></returns>
        IPayrollHistoryQuery ByPayrollId(int payrollId);

        /// <summary>
        /// Filters by payrolls belonging to a particular client.
        /// </summary>
        /// <param name="clientId">ID of client to get payroll info for.</param>
        /// <returns></returns>
        IPayrollHistoryQuery ByClientId(int clientId);

        /// <summary>
        /// Filters by payrolls belonging to particular clients.
        /// </summary>
        /// <param name="clientId">ID of clients to get payroll info for.</param>
        /// <returns></returns>
        IPayrollHistoryQuery ByClientIds(int[] clientId);

        /// <summary>
        /// Filters by payrolls of the specified type(s).
        /// </summary>
        /// <param name="types">Type(s) of payrolls to get.</param>
        /// <returns></returns>
        IPayrollHistoryQuery ByPayrollRunType(params PayrollRunType[] types);

        /// <summary>
        /// Orders the query results by check date.
        /// </summary>
        /// <param name="direction">Direction to sort by.</param>
        /// <returns></returns>
        IPayrollHistoryQuery SortByCheckDate(SortDirection direction = SortDirection.Ascending);

        /// <summary>
        /// Filters the payroll results by modified date.
        /// </summary>
        /// <param name="modifiedDate">The modified date to filter by.</param>
        /// <returns></returns>
        IPayrollHistoryQuery ByModifiedDate(DateTime modifiedDate);
    }
}