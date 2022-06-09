using System;

namespace Dominion.Core.Dto.Labor
{
    public interface IHasPunchShiftDateInfo
    {
        DateTime? ShiftDate { get; }
        DateTime ModifiedPunch { get; }
        byte? TransferOption { get; }
    }
}