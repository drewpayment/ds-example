using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;
using Dominion.LaborManagement.Dto.GroupScheduling;

namespace Dominion.Domain.Entities.Labor
{
    public class ScheduleGroupShiftName : Entity<ScheduleGroupShiftName>, IHasModifiedData
    {
        public virtual int    ScheduleGroupShiftNameId  { get; set; } 
        public virtual int    ClientId                  { get; set; } 
        public virtual int    ScheduleGroupId           { get; set; } 
        public virtual string Name                      { get; set; } 

        public virtual int        ModifiedBy   { get; set; } 
        public virtual DateTime   Modified     { get; set; } 

        public virtual ScheduleGroup ScheduleGroup { get; set; }
    }
}
