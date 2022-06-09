using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IPaycheckTaxHistoryQuery : IQuery<PaycheckTaxHistory, IPaycheckTaxHistoryQuery>
    {
        /// <summary>
        /// Filters history for a single client.
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        IPaycheckTaxHistoryQuery ByClientId(int clientId);

        IPaycheckTaxHistoryQuery ByEmployeeId(int employeeId);

        //IPaycheckTaxHistoryQuery ByPaycheckHistoryIds(IEnumerable<int> paycheckPayDataHistoryIds);

        /// <summary>
        /// Filters history by Tax Type
        /// </summary>
        /// <param name="taxType">input: char. Tax Type to filter by</param>
        /// <returns></returns>
        IPaycheckTaxHistoryQuery ByTaxType(string taxType);

        /// <summary>
        /// Gets Paycheck Pay Data by PayrollID
        /// </summary>
        /// <param name="payrollId"></param>
        /// <returns></returns>
        IPaycheckTaxHistoryQuery ByPayrollId(int payrollId);

        /// <summary>
        /// Gets Paycheck Pay Data by PayrollID
        /// </summary>
        /// <param name="payrollId"></param>
        /// <returns></returns>
        IPaycheckTaxHistoryQuery ByGenPaycheckPayDataHistoryID(int? genPaycheckPayDataHistoryID);

        /// <summary>
        /// Gets Paycheck Pay Data between 2 dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IPaycheckTaxHistoryQuery ByDateRange(DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// Filters history for a single client tax.
        /// </summary>
        /// <param name="clientId">ID of client tax to filter by.</param>
        /// <returns></returns>
        IPaycheckTaxHistoryQuery ByClientTaxId(int clientTaxId);
    }
}
