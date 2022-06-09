using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public partial class ClockClientScheduleSelectedDto
    {
        public int EmployeeId { get; set; }
        public int ClockClientScheduleId { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
        public int ClientId { get; set; }
    }
}
