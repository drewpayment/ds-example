using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Reporting
{
    public enum ReportQueueLogTypeEnum : byte
    {
        Blank               = 0,
        ThreadTimedOut      = 1,
        TooManyUserThreads  = 2,
        TooManyTotalThreads = 3,
        ReportServerError   = 4,
    }
}
