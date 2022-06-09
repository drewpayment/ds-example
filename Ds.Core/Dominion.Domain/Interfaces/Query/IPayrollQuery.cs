using System;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IPayrollQuery : IQuery<Domain.Entities.Payroll.Payroll, IPayrollQuery>
    {
        /// <summary>
        /// Filters the payroll results by the unique Payroll ID.
        /// </summary>
        /// <param name="payrollId">Payroll ID to filter the payroll results by.</param>
        /// <returns></returns>
        IPayrollQuery ByPayrollId(int payrollId);

        /// <summary>
        /// Filters by payrolls belonging to a particular client.
        /// </summary>
        /// <param name="clientId">ID of client to get payroll info for.</param>
        /// <returns></returns>
        IPayrollQuery ByClientId(int clientId);

        /// <summary>
        /// Filters payroll results based on payroll status
        /// </summary>
        /// <param name="isOpen">status of the payroll</param>
        /// <returns></returns>
        IPayrollQuery ByStatus(bool isOpen);
        /// <summary>
        /// Filters by payrolls of the specified type(s).
        /// </summary>
        /// <param name="types">Type(s) of payrolls to get.</param>
        /// <returns></returns>
        IPayrollQuery ByPayrollRunType(params PayrollRunType[] types);
        IPayrollQuery ByPayrollRunTypeIsNot(params PayrollRunType[] types);
        IPayrollQuery ByIsOpen(bool isOpen);
        IPayrollQuery OrderByCheckDateAndPayrollId();

        /// <summary>
        /// Orders the query results by check date.
        /// </summary>
        /// <param name="direction">Direction to sort by.</param>
        /// <returns></returns>
        IPayrollQuery SortByCheckDate(SortDirection direction = SortDirection.Ascending);
        IPayrollQuery SortByPeriodEnded(SortDirection dir = SortDirection.Ascending);
        IPayrollQuery ByActivityDateInPeriod(DateTime date);
        IPayrollQuery ByDateRange(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Filters by payrolls off payrollrunId.
        /// </summary>
        /// <param name="payrollRunId">payrollrunId of payroll info looking for.</param>
        /// <returns></returns>
        IPayrollQuery ByPayrollRunId(PayrollRunType payrollRunId);

        /// <summary>
        /// Filters by greater than initial date.
        /// </summary>
        /// <param name="initDate">inital date of payroll info looking for.</param>
        /// <returns></returns>
        IPayrollQuery ByGreaterThanDate(DateTime initDate);

        /// <summary>
        /// Filters by less than final date.
        /// </summary>
        /// <param name="finalDate">final date of payroll info looking for.</param>
        /// <returns></returns>
        IPayrollQuery ByLessThanDate(DateTime finalDate);
        IPayrollQuery ByPayFrequency(PayFrequencyType payFrequency);
    }
}
