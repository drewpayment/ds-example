using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    /// <summary>
    /// Defines a class for objects that store the total data model for the form.
    /// </summary>
    public class Totals
    {
        /// <summary>
        /// The filters and settings that are being used.
        /// </summary>
        public TotalsFilters Filters { get; set; }

        /// <summary>
        /// The array of employee totals that this totals object displays.
        /// </summary>
        public virtual IEnumerable<EmployeeTotals> Employees { get; set; }
    }
}
