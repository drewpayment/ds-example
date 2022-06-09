using Dominion.Core.Dto.LeaveManagement;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query.Labor
{
    public interface ILeaveManagementPendingAwardQuery : IQuery<LeaveManagementPendingAward, ILeaveManagementPendingAwardQuery>
    {
        //ILeaveManagementPendingAwardQuery ByEmployeeId(int employeeId);
        ILeaveManagementPendingAwardQuery ByClientId(int clientId);
        ILeaveManagementPendingAwardQuery ByClientAccrualId(int clientAccrualId);
        //ILeaveManagementPendingAwardQuery ByEmployeeIdAndClientAccrualId(int employeeId, int clientAccrualId);
        ILeaveManagementPendingAwardQuery ByTimeOffAwardType(TimeOffAwardType timeOffAwardType);

    }
}
