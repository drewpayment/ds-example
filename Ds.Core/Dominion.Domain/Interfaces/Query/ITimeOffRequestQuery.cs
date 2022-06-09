using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dominion.Core.Dto.LeaveManagement;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface ITimeOffRequestQuery : IQuery<TimeOffRequest, ITimeOffRequestQuery>
    {
        ITimeOffRequestQuery ByClientId(int clientId);

        ITimeOffRequestQuery RequestFromGreaterThan(DateTime date);

        ITimeOffRequestQuery ByPending();
        ITimeOffRequestQuery RequestFromLessThan(DateTime date);
        ITimeOffRequestQuery ByEmployeeId(int employeeId);
        ITimeOffRequestQuery ByEmployeeIds(IEnumerable<int> empIds);
        ITimeOffRequestQuery ExcludeStatus(TimeOffStatusType type);
        ITimeOffRequestQuery ExcludeRequest(int timeOffRequestId);
        ITimeOffRequestQuery ByRequestTimeOffId(int requestTimeOffId);
        ITimeOffRequestQuery ByDateRange(DateTime startDate, DateTime endDate);
    }
}
