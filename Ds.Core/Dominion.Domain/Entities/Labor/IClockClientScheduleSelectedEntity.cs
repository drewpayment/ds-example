using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Labor
{
    public interface IClockClientScheduleSelectedEntity
    {
        int EmployeeId { get; set; }
        int ClockClientScheduleId { get; set; }
        int ModifiedBy { get; set; }
        DateTime Modified { get; set; }
        int ClientId { get; set; }
    }
}
