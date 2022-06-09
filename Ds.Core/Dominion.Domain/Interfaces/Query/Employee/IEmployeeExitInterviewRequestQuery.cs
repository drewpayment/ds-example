using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Benefit;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.ExitInterview
{
    public interface IEmployeeExitInterviewRequestQuery : IQuery<EmployeeExitInterviewRequest, IEmployeeExitInterviewRequestQuery>
    {
        IEmployeeExitInterviewRequestQuery ByEmployeeId(int employeeId);
        IEmployeeExitInterviewRequestQuery ByNotSent();

    }
}
