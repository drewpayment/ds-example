using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public class EmployeeDefaultShift : Entity<EmployeeDefaultShift>, IHasModifiedData
    {
        public virtual int EmployeeDefaultShiftId   { get; set; } 
        public virtual int EmployeeId               { get; set; } 
        public virtual int GroupScheduleShiftId     { get; set; } 
        public virtual int        ModifiedBy           { get; set; } 
        public virtual DateTime   Modified           { get; set; }

        public virtual GroupScheduleShift GroupScheduleShift { get; set; }
        public virtual Employee.Employee Employee { get; set; }
    }
}
