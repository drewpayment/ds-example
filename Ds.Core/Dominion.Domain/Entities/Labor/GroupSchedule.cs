using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public class GroupSchedule: Entity<GroupSchedule>, IHasModifiedData
    {
        public virtual int      GroupScheduleId { get; set; } 
        public virtual int      ClientId        { get; set; } 
        public virtual string   Name            { get; set; } 
        public virtual bool     IsActive        { get; set; }

        public virtual int        ModifiedBy           { get; set; } 
        public virtual DateTime   Modified           { get; set; } 

        public virtual Client Client { get; set; }

        public virtual ICollection<GroupScheduleShift> GroupScheduleShifts { get; set; }
    }
}
