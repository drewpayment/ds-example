using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public partial class ClockClientScheduleSelected : Entity<ClockClientScheduleSelected>,
        IHasModifiedData,
        IClockClientScheduleSelectedEntity,
        IHasChangeHistoryEntityWithEnum<ClockClientScheduleSelectedChangeHistory>
    {
        public virtual int EmployeeId { get; set; }
        public virtual int ClockClientScheduleId { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ClientId { get; set; }

        public ClockClientScheduleSelected()
        {
        }

        public virtual Employee.Employee Employee { get; set; }
        public virtual ClockClientSchedule ClockSchedule { get; set; }
    }
}
