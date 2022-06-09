using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public partial class ClockEmployeeAllocateHoursDetail : Entity<ClockEmployeeAllocateHoursDetail>, IHasModifiedData
    {
        public virtual int ClockEmployeeAllocateHoursDetailId { get; set; } 
        public virtual int ClockEmployeeAllocateHoursId { get; set; } 
        public virtual int ClientEarningId { get; set; } 
        public virtual int ClientCostCenterId { get; set; } 
        public virtual double Hours { get; set; } 
        public virtual bool IsOvertime { get; set; } 
        public virtual DateTime Modified { get; set; } 
        public virtual int ModifiedBy { get; set; } 
        public virtual int? ClientShiftId { get; set; } 
        public virtual ClockEmployeeAllocateHours ClockEmployeeAllocateHours { get; set; }
    }
}
