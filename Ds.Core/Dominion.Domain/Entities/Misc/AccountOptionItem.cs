using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Misc
{
    /// <summary>
    /// Represents a single list item for an AccountOption with DataType of List.
    /// </summary>
    public class AccountOptionItem : Entity<AccountOptionItem>
    {
        public virtual int AccountOptionItemId { get; set; }
        public virtual AccountOption AccountOption { get; set; }
        public virtual AccountOptionInfo AccountOptionInfo { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsDefault { get; set; }
        public virtual byte? Value { get; set; }

        public AccountOptionItem()
        {
        }
    }

    /// <summary>
    /// Enum that represents the values 
    /// for Time Clock categories Use Shift options.
    /// </summary>
    public enum TimeClockUseShiftOptionItems
    {
        None = 0,
        Manual = 1,
        AutomaticSplits = 2,
        ApplyPremiumToShiftsWithStartStopTimes = 3
    }
}