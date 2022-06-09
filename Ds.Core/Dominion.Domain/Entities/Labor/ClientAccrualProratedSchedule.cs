using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public class ClientAccrualProratedSchedule : Entity<ClientAccrualProratedSchedule>, IHasModifiedData
    {
        public virtual int      ClientAccrualProratedScheduleId { get; set; }
        public virtual int      ClientId                        { get; set; }
        public virtual int      ClientAccrualId                 { get; set; }
        public virtual DateTime ScheduleFrom                    { get; set; }
        public virtual DateTime ScheduleTo                      { get; set; }
        public virtual int?      AwardAfterDaysValue             { get; set; }
        public virtual DateTime? AwardOnDate                     { get; set; }
        public virtual decimal  AwardValue                      { get; set; }
        public virtual DateTime Modified                        { get; set; }
        public virtual int      ModifiedBy                      { get; set; }

        public virtual Client        Client        { get; set; }
        public virtual ClientAccrual ClientAccrual { get; set; }
    }
}
