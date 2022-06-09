using System.Collections.Generic;
using Dominion.Domain.Entities.Aca;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IAca1095CLineItemQuery : IQuery<Aca1095CLineItem, IAca1095CLineItemQuery>
    {
        /// <summary>
        /// Filters 1095C data by employee.
        /// </summary>
        /// <param name="employeeId">Employee to filter by.</param>
        /// <returns>Query to be further filtered.</returns>
        IAca1095CLineItemQuery ByEmployeeId(int employeeId);

        /// <summary>
        /// Filters 1095C data by year.
        /// </summary>
        /// <param name="year">Year to filter by.</param>
        /// <returns>Query to be further filtered.</returns>
        IAca1095CLineItemQuery ByYear(int year);

        /// <summary>
        /// Get existing line items only matching the employee ids passed in.
        /// </summary>
        /// <param name="empIds">List of employee ids</param>
        /// <returns></returns>
        IAca1095CLineItemQuery ByEmployeeIds(IEnumerable<int> empIds);

        /// <summary>
        /// Gets 1095C detail records that do not have an associated header record.
        /// This should never happen, but has for a client or two.
        /// </summary>
        /// <returns></returns>
        IAca1095CLineItemQuery ByHasMissingHeaderRecord();

    }
}
