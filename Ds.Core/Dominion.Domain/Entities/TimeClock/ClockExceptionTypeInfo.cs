using Dominion.Core.Dto.Labor;
using Dominion.Domain.Entities.Base;
using Dominion.LaborManagement.Dto.Clock;

namespace Dominion.Domain.Entities.TimeClock
{
    public class ClockExceptionTypeInfo : Entity<ClockExceptionTypeInfo>
    {
        public ClockExceptionType           ClockExceptionTypeId { get; set; } 
        public string                       ClockException       { get; set; } 
        public bool?                        IsHours              { get; set; } 
        public bool                         IsHasPunchTimeOption { get; set; } 
        public bool                         IsHasAmountTextBox   { get; set; } 
        public ClockExceptionGroupType      ClockExceptionType   { get; set; }
    }
}
