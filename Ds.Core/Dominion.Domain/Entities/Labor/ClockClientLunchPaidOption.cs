using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public partial class ClockClientLunchPaidOption : Entity<ClockClientLunchPaidOption>, IHasModifiedOptionalData
    {
        public virtual int ClockClientLunchPaidOptionId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int? ClockClientLunchId { get; set; }
        public virtual double? FromMinutes { get; set; }
        public virtual double? ToMinutes { get; set; }
        public virtual byte? ClockClientLunchPaidOptionRulesId { get; set; }
        public virtual double? OverrideMinutes { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual int? ModifiedBy { get; set; }

        public virtual ClockClientLunch ClockClientLunch { get; set; }
        
    }
}
