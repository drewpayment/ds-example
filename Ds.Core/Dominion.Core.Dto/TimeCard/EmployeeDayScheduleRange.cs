using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class EmployeeDayScheduleRange
    {
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }
        
        public override string ToString()
        {
            return string.Format("{0:h:mmtt}-{1:h:mmtt}", StartTime, StopTime).ToLowerInvariant();
        }
    }
}
