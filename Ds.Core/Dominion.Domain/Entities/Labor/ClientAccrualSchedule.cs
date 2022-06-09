using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public partial class ClientAccrualSchedule : Entity<ClientAccrualSchedule>, IHasModifiedOptionalData
    {
        public virtual int ClientAccrualScheduleId { get; set; }
        public virtual int ClientAccrualId { get; set; }
        public virtual int ServiceStart { get; set; }
        public virtual int ServiceStartFrequencyId { get; set; }
        public virtual int ServiceFrequencyId { get; set; }
        public virtual int? ServiceEnd { get; set; }
        public virtual int? ServiceEndFrequencyId { get; set; }
        public virtual double? AccrualBalanceLimit { get; set; }
        public virtual double? BalanceLimit { get; set; }
        public virtual double? Reward { get; set; }
        public virtual int ServiceRewardFrequencyId { get; set; }
        public virtual int? ServiceRenewFrequencyId { get; set; }
        public virtual int? RenewEnd { get; set; }
        public virtual double? CarryOver { get; set; }
        public virtual int? ServiceCarryOverFrequencyId { get; set; }
        public virtual int? ServiceCarryOverWhenFrequencyId { get; set; }
        public virtual int? ServiceCarryOverTill { get; set; }
        public virtual int? ServiceCarryOverTillFrequencyId { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual double? RateCarryOverMax { get; set; }

        public virtual ClientAccrual                  ClientAccrual                  { get; set; }
    }
}
