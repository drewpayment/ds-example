using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IGetClockPayrollListByClientIDPayrollRunIDQuery : IQuery<IGetClockPayrollListByClientIDPayrollRunIDQuery>
    {


        IGetClockPayrollListByClientIDPayrollRunIDQuery ByClientID(int clientID);

        IGetClockPayrollListByClientIDPayrollRunIDQuery ByPayrollRunID(int payrollRunID);

        IGetClockPayrollListByClientIDPayrollRunIDQuery ByHideCustomDateRange(bool hideCustomDateRange);

        //IClockEmployeePunchQuery ByClientId(int clientId);

        //IClockEmployeePunchQuery ByEmployeeId(int employeeId);

        //IClockEmployeePunchQuery ByDates(DateTime startDate, DateTime endDate, bool attemptToUseShiftDate = false);

        //IClockEmployeePunchQuery ByCostCenter(int? costCenterId);
        //IClockEmployeePunchQuery OrderByRawPunch(SortDirection direction);
    }
}
