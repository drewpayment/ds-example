﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IGetClockEmployeeApproveHoursOptionsQuery : IQuery<IGetClockEmployeeApproveHoursOptionsQuery>
    {

        IGetClockEmployeeApproveHoursOptionsQuery ByControlID(int controlID);

    }
}