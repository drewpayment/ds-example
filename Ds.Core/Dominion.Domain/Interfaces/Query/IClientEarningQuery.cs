using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominion.Core.Dto.Payroll;
using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.Payroll;
using Dominion.Pay.Dto.Earnings;
using Dominion.Utility.Query;
using ClientEarningCategory = Dominion.Core.Dto.Payroll.ClientEarningCategory;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Represents a query that can be executed on a set of client earnings.
    /// </summary>
    public interface IClientEarningQuery : IQuery<ClientEarning, IClientEarningQuery>
    {
        /// <summary>
        /// Filters the earnings by client ID.
        /// </summary>
        /// <param name="clientId">Unique client ID to filter earnings by.</param>
        /// <returns>Query to be further built.</returns>
        IClientEarningQuery ByClientId(int clientId);

        /// <summary>
        /// Filters the earnings by the EarningCategoryId
        /// </summary>
        /// <param name="earningCategoryIds">id for the Earning Category</param>
        /// <returns></returns>
        IClientEarningQuery ByEarningCategoryIds(params ClientEarningCategory[] earningCategoryIds);

        /// <summary>
        /// Filters the earnings by earning code.
        /// </summary>
        /// <param name="code">Code to filter earnings by.</param>
        /// <returns>Query to be further built.</returns>
        IClientEarningQuery ByEarningCode(string code);

        /// <summary>
        /// Filters the earnings by unique ID.
        /// </summary>
        /// <param name="clientEarningId">Unique client earning ID to filter by.</param>
        /// <returns>Query to be further built.</returns>
        IClientEarningQuery ByClientEarningId(int clientEarningId);

        /// <summary>
        /// Retrieves multiple earnings by unique ID.
        /// </summary>
        /// <param name="earningIds">Unique client earning IDs to filter by.</param>
        /// <returns>Query to be further built.</returns>
        IClientEarningQuery ByClientEarningIds(IEnumerable<int> earningIds);

        /// <summary>
        /// Filters based on an earning's active state.
        /// </summary>
        /// <param name="isActive">Indication if the earning is currently active.</param>
        /// <returns>Query to be further built.</returns>
        IClientEarningQuery ByActivity(bool isActive);

        IClientEarningQuery ByEarningCategoryId(ClientEarningCategory earningCategoryId);

        IClientEarningQuery ByIsBlockedFromTimeClock(bool isBlockedFromTimeClock = true, IEnumerable<UserType> allowedUserTypes = null);

        IClientEarningQuery ByIsEmergencyLeaveEarning(bool isEmergencyLeave = true);

        IClientEarningQuery OrderByDescription(SortDirection direction);

        IClientEarningQuery ByIsReimburseTaxes(bool isReimburseTaxes = true);

        //IClientEarningQuery HasPayCheckHistory();

        //IClientEarningQuery ByPayrollControlTotalPayrollId(int payrollId);
    }
}