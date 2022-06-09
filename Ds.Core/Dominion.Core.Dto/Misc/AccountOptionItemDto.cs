using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Misc
{
    public class AccountOptionItemDto
    {
        public int AccountOptionItemId                { get; set; }
        public AccountOption AccountOption            { get; set; }
        public AccountOptionInfoDto AccountOptionInfo { get; set; }
        public string Description                     { get; set; }
        public bool IsDefault                         { get; set; }
        public byte? Value                            { get; set; }
    }

    /// <summary>
    /// Enum that represents the values 
    /// for Time Clock categories Use Shift options.
    /// </summary>
    public enum TimeClockUseShiftOptionItems
    {
        None                                   = 0,
        Manual                                 = 1,
        AutomaticSplits                        = 2,
        ApplyPremiumToShiftsWithStartStopTimes = 3
    }
}
