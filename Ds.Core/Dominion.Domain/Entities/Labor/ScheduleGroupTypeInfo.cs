using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.LaborManagement.Dto.GroupScheduling;

namespace Dominion.Domain.Entities.Labor
{
    public class ScheduleGroupTypeInfo : Entity<ScheduleGroupTypeInfo>
    {
        public virtual ScheduleGroupType ScheduleGroupType { get; set; } 
        public virtual string Name { get; set; } 
    }
}
