using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.SftpUpload.CsvTemplates
{
    public class OnShiftScheduleLayout
    {
        // LocationID =  Cost Center Code
        // EmployeeID = EmployeeNumber 
        // StartTime = Schedule Start
        // StopTime = Schedule Stop
        public string costCenterId { get; set; }
        public string employeeId { get; set; }
        public DateTime scheduleStart { get; set; }
        public DateTime scheduleStop { get; set; }
    }
}
