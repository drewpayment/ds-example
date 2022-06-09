using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public enum ApprovalStatus : byte
    {
        Pending = 1,
        Approved = 2,
        Declined = 3,
        NoRequest = 4
    }
}
