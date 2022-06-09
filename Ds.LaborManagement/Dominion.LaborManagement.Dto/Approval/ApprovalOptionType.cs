using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Approval
{
    public enum ApprovalOptionType
    {
        None             = 1,
        HoursAndBenefits = 2,
        Exceptions       = 3,
        Everyday         = 4,
        AllActivity      = 5,
        CostCenter       = 6
    }
}
