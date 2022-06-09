using System;
using System.CodeDom;
using System.Collections.Generic;

namespace Dominion.Core.Dto.LeaveManagement
{
    public class GetActiveTimeOffEventsSprocDto
    {
        // DATA SET 1
        public IEnumerable<TimeOffEventSprocDto> PolicyChangeEvents { get; set; }
        
        //  DATA SET 2
        public DateTime LastPayrollDate { get; set; } 
        public DateTime YearBefore { get; set; }
    }
}
