using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class EmployeePointListByDate
    {
        public class Point
        {
            public int EmployeeID { get; set; }
            public DateTime DateOccured { get; set; }
            public int ClockExceptionID { get; set; }
            public int ClockEmployeeExceptionHistoryID { get; set; }
            public double Amount { get; set; }
        }

        public IEnumerable<Point> Points { get; set; }
    }
}
