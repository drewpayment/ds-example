using Dominion.Core.Dto.Labor;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.TimeClock
{
    public partial class ClockRoundingType : Entity<ClockRoundingType>
    {
        public virtual PunchRoundingType ClockRoundingTypeId { get; set; }
        public virtual string            Description         { get; set; }
        public virtual double?           Minutes             { get; set; }
        public virtual byte?             RoundDirection      { get; set; }
    }
}