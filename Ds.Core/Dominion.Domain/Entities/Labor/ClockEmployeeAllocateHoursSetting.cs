using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public class ClockEmployeeAllocateHoursSetting : Entity<ClockEmployeeAllocateHoursSetting>, IHasModifiedData
    {
        public virtual int ClockEmployeeAllocateHoursSettingsId { get; set; } 
        public virtual int UserId { get; set; } 
        public virtual byte MinimumAllocators { get; set; } 
        public virtual int ModifiedBy { get; set; } 
        public virtual DateTime Modified { get; set; } 
    }
}