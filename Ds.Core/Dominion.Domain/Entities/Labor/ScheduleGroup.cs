using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;
using Dominion.LaborManagement.Dto.GroupScheduling;

namespace Dominion.Domain.Entities.Labor
{
    public class ScheduleGroup : Entity<ScheduleGroup>
    {
        public virtual int ScheduleGroupId { get; set; } 
        public virtual ScheduleGroupType ScheduleGroupType { get; set; } 
        public virtual ScheduleGroupTypeInfo ScheduleGroupTypeInfo { get; set; } 
        public virtual int? ClientCostCenterId { get; set; } 

        public virtual ClientCostCenter ClientCostCenter { get; set; }
        public virtual ICollection<ScheduleGroupShiftName> ScheduleGroupShiftNames { get; set; }
    }
}
