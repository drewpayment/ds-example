using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Payroll;

namespace Dominion.Core.Dto.Labor
{
    public class CompanyHolidayDto
    {
        public List<ClockClientHolidayDto> HolidayRules { get; set; }
        public List<ClientEarningDto> HolidayEarnings { get; set; }
        public List<ClientEarningDto> HolidayWorkedEarnings { get; set; }
        public List<HolidayDateDto> DefaultHolidays { get; set; }
        
    }
}
