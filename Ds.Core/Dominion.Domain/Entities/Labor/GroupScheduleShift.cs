using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public class GroupScheduleShift : Entity<GroupScheduleShift>, IHasModifiedData
    {
        public virtual int          GroupScheduleShiftId      { get; set; } 
        public virtual int          GroupScheduleId           { get; set; } 
        public virtual int?         ScheduleGroupShiftNameId  { get; set; } 
        public virtual int          ScheduleGroupId           { get; set; } 
        public virtual TimeSpan     StartTime                 { get; set; } 
        public virtual TimeSpan     EndTime                   { get; set; } 
        public virtual DayOfWeek    DayOfWeek                 { get; set; } 
        public virtual byte         NumberOfEmployees         { get; set; }
        public virtual bool         IsDeleted                 { get; set; }

        public virtual int        ModifiedBy    { get; set; } 
        public virtual DateTime   Modified      { get; set; } 

        public virtual GroupSchedule          GroupSchedule          { get; set; }
        public virtual ScheduleGroup          ScheduleGroup          { get; set; }
        public virtual ScheduleGroupShiftName ScheduleGroupShiftName { get; set; }

        public virtual ICollection<EmployeeDefaultShift> EmployeeDefaultShifts { get; set; }
        public virtual ICollection<EmployeeSchedulePreview> EmployeeSchedulePreviews { get; set; }
    }
}
