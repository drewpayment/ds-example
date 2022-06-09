using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public enum ReferenceDate : byte
    {
        /// <summary>
        /// No reference date. Reviews are created using date ranges manually entered by user.  Does not carry over from year to year.
        /// </summary>
        HardCodedRange = 1,
        /// <summary>
        /// The reference date is a certain day of year (such as 7/4). This same day of year carries over from year to year.
        /// </summary>
        CalendarYear,
        /// <summary>
        /// The reference date is the date the employee was hired/rehired
        /// </summary>
        DateOfHire
    }
}
