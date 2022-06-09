using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Payroll;

namespace Dominion.Core.Dto.Labor
{
    public class ClockClientAddHoursViewDto
    {
        public List<ClockClientAddHoursDto> ClockClientAddHoursDtos { get; set; }
        public List<ClientEarningDto> ClientEarningDtos { get; set; }
    }
}
