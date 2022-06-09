using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    public interface IEmployeeBenefitInfoQuery : IQuery<EmployeeBenefitInfo, IEmployeeBenefitInfoQuery>
    {
        IEmployeeBenefitInfoQuery ByEmployeeId(int employeeId);
        
    }
}
