using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public class ClockEmployeeAllocateHoursOverride : Entity<ClockEmployeeAllocateHoursOverride>, IHasModifiedData
    {
        public virtual int ClockEmployeeAllocateHoursOverrideId { get; set; } 
        public virtual int ClockEmployeeAllocateHoursId { get; set; } 
        public virtual int ClientEarningId { get; set; } 
        public virtual int ClientCostCenterId { get; set; } 
        public virtual double Hours { get; set; } 
        public virtual DateTime Modified { get; set; } 
        public virtual int ModifiedBy { get; set; }
        public virtual ClockEmployeeAllocateHours ClockEmployeeAllocateHours { get; set; }
    }
}