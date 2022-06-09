using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class ExceptionHistoryByEmployeeID
    {
        public class History
        {
            public int ClockEmployeeExceptionHistoryID { get; set; }
            public int EmployeeID { get; set; }
            public int ClockExceptionID { get; set; }
            public double? Hours { get; set; }
            public DateTime EventDate { get; set; }
            public int? ClockClientExceptionDetailID { get; set; }
            public int? ClockEmployeePunchID { get; set; }
            public int? ClockClientLunchID { get; set; }
            public int ClientID { get; set; }
            public string ClockException { get; set; }
            public bool IsApproved { get; set; }
        }

        public IEnumerable<History> Exceptions { get; set; }
    }
}
