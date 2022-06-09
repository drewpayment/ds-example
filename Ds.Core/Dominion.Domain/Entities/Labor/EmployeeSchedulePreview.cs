using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public class EmployeeSchedulePreview : Entity<EmployeeSchedulePreview>, IHasModifiedData
    {
        public virtual int        EmployeeSchedulePreviewId   { get; set; } 
        public virtual int        GroupScheduleShiftId        { get; set; } 
        public virtual int        EmployeeId                  { get; set; } 
        public virtual DateTime   EventDate                   { get; set; } 
        //public virtual TimeSpan   StartTime                   { get; set; } 
        //public virtual TimeSpan   EndTime                     { get; set; } 
        public virtual int        ModifiedBy                  { get; set; } 
        public virtual DateTime   Modified                    { get; set; } 
        public virtual int?       Override_ScheduleGroupId    { get; set; }

        public virtual Employee.Employee Employee { get; set; }
        public virtual GroupScheduleShift GroupScheduleShift { get; set; } 
        public virtual ScheduleGroup Override_ScheduleGroup { get; set; }
        

    }
}
