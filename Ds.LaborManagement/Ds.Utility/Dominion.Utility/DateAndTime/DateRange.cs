using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.DateAndTime
{
    public class DateRange
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        /// <summary>
        /// GTE AND LTE.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool IsBetween(DateTime dt)
        {
            return dt >= Start && dt <= End;
        }

    }
}
