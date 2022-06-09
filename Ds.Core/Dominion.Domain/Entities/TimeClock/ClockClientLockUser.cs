using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using System;

namespace Dominion.Domain.Entities.TimeClock
{
    public class ClockClientLockUser : Entity<ClockClientLockUser>, IHasModifiedData
    {
        public int ClockClientLockUserId { get; set; }
        public bool IsTimecardLocked { get; set; }
        public int PayrollId { get; set; }
        public int ClientId { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
    }
}
