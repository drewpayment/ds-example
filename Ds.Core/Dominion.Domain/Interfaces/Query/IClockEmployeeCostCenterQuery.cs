﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockEmployeeCostCenterQuery : IQuery<ClockEmployeeCostCenter, IClockEmployeeCostCenterQuery>
    {
        IClockEmployeeCostCenterQuery ByCostCenterId(int costCenterId);

        IClockEmployeeCostCenterQuery ByClientId(int clientId);

        IClockEmployeeCostCenterQuery ByEmployeeId(int employeeId);
    }
}