using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public partial class ClockEmployeeAllocateHours : Entity<ClockEmployeeAllocateHours>, IHasModifiedData
    {
        public virtual int ClockEmployeeAllocateHoursId { get; set; } 
        public virtual int EmployeeId { get; set; } 
        public virtual int ClientId { get; set; } 
        public virtual DateTime StartDate { get; set; } 
        public virtual DateTime EndDate { get; set; } 
        public virtual DateTime Modified { get; set; } 
        public virtual int ModifiedBy { get; set; } 
        public virtual string Notes { get; set; } 
        public virtual ICollection<ClockEmployeeAllocateHoursDetail> Details { get; set; }
        public virtual ICollection<ClockEmployeeAllocateHoursOverride> Overrides { get; set; }
    }
}