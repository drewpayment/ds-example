using System.Collections;
using System.Collections.Generic;

using Dominion.Aca.Dto.Forms;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Onboarding;
using Dominion.Domain.Interfaces.Repositories;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IOnboardingW4AssistQuery : IQuery<EmployeeOnboardingW4Assist, IOnboardingW4AssistQuery>
    {
        /// <summary>
        /// Filters employees by the specified ID.
        /// </summary>
        /// <param name="employeeId">ID of employee.</param>
        /// <returns></returns>
        IOnboardingW4AssistQuery ByEmployeeId(int employeeId);
    }
}
