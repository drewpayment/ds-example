using System;

namespace Dominion.Core.Dto.Employee
{
    [Flags]
    public enum PayType : byte
    {
        Hourly = 1,
        Salary = 2
    }
}