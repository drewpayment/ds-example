using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class TimeCardRowDto
    {
        public IEnumerable<EmployeeTimeCard> EmployeeTimeCards { get; set; }
    }
}
