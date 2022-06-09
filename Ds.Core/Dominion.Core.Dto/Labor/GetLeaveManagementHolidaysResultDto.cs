using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public class GetLeaveManagementHolidaysResultDto
    {
        public string HolidayName { get; set; }
        public DateTime HolidayDate { get; set; }
        public decimal HolidayHours { get; set; }
        public decimal TotalHoursAvailable { get; set; }
    }
}
