using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Sprocs;

namespace Dominion.Core.Dto.Labor
{
    public class TimeCardAuthorizationSaveArgs
    {
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public int ModifiedBy { get; set; }
        public List<dataEntry> DataEntries { get; set; }
        public List<int> EmployeeIds { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool isRecalcActivity { get; set; }
    }

    public class dataEntry
    {
        public InsertClockEmployeeApproveDateArgsDto ClockEmployeeApproveDate { get; set; }
        public bool DoRecalcPoints { get; set; }
    }
}
