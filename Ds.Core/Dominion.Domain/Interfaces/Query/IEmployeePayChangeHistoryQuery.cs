using System;

using Dominion.Domain.Entities.Employee;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeePayChangeHistoryQuery : IEmployeePayBaseQuery<EmployeePayChangeHistory, IEmployeePayChangeHistoryQuery>
    {
        /// <summary>
        /// Filters the change history to include only the most recent records for each employee. NOTE: If
        /// applying additional filters, apply them BEFORE calling this method to ensure the best performance.  Typically,
        /// this will called in conjunction with <see cref="ByChangedOnOrBefore"/>.
        /// </summary>
        /// <returns></returns>
        IEmployeePayChangeHistoryQuery IncludeMostRecentChangesOnly();

        /// <summary>
        /// Filters by changes made on or before the given date.
        /// </summary>
        /// <param name="date">Date the change history must be less-than-or-equal-to.</param>
        /// <returns></returns>
        IEmployeePayChangeHistoryQuery ByChangedOnOrBefore(DateTime date);
    }
}
