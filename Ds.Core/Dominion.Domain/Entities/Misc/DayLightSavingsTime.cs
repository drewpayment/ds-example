using System;

namespace Dominion.Domain.Entities.Misc
{
    public partial class DayLightSavingsTime
    {
        public virtual int DayLightSavingsTimeId { get; set; }
        public virtual DateTime BeginDate { get; set; }
        public virtual DateTime EndDate { get; set; }
    }
}