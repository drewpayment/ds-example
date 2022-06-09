using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IGetClockEmployeeApproveDateQuery : IQuery<IGetClockEmployeeApproveDateQuery>
    {

        IGetClockEmployeeApproveDateQuery ByClientID(int clientID);
        IGetClockEmployeeApproveDateQuery ByUserID(int userID);
        IGetClockEmployeeApproveDateQuery ByEmployeeID(int employeeID);
        IGetClockEmployeeApproveDateQuery ByStartDate(DateTime startDate);
        IGetClockEmployeeApproveDateQuery ByEndDate(DateTime endDate);
        IGetClockEmployeeApproveDateQuery ByOnlyPayToSchedule(bool onlyPayToSchedule);

    }
}
