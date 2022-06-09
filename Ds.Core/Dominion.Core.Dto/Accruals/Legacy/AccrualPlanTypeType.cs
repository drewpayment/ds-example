using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Accruals.Legacy
{
    // Copied from dsCalculateAccrual.AccrualType
    public enum AccrualPlanTypeType
    {
        AutoAccrual = 1,
        ManualAccrual = 2,
        CustomAccrual = 3,
        PayOut = 4,
        PayOutExtraCheck = 5,
    }
}
