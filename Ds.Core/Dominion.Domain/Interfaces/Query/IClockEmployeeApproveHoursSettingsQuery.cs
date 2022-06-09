using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockEmployeeApproveHoursSettingsQuery : IQuery<ClockEmployeeApproveHoursSettings, IClockEmployeeApproveHoursSettingsQuery>
    {


        IClockEmployeeApproveHoursSettingsQuery ByClientID(int clientID);
        IClockEmployeeApproveHoursSettingsQuery ByUserID(int userID);


        //IClockEmployeePunchQuery ByClientId(int clientId);

        //IClockEmployeePunchQuery ByEmployeeId(int employeeId);

        //IClockEmployeePunchQuery ByDates(DateTime startDate, DateTime endDate, bool attemptToUseShiftDate = false);

        //IClockEmployeePunchQuery ByCostCenter(int? costCenterId);
        //IClockEmployeePunchQuery OrderByRawPunch(SortDirection direction);
    }
}
