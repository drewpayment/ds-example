using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class PayPeriodEnded
    {
        public class Period
        {
            public int EmployeeID { get; set; }
            public DateTime PeriodEnded { get; set; }
            public DateTime PeriodStart_Locked { get; set; }
            public string WarningMessage_Locked { get; set; }
            public string WarningMessage_Closed { get; set; }
            public bool AllowScheduleEdits { get; set; }
        }

        public IEnumerable<Period> Rows { get; set; }
    }
}
