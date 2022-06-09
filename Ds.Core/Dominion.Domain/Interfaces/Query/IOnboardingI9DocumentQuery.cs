using System.Collections;
using System.Collections.Generic;

using Dominion.Aca.Dto.Forms;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Onboarding;
using Dominion.Domain.Interfaces.Repositories;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IOnboardingI9DocumentQuery : IQuery<EmployeeOnboardingI9Document, IOnboardingI9DocumentQuery>
    {
        /// <summary>
        /// Filters employees by the specified ID.
        /// </summary>
        /// <param name="employeeId">ID of employee.</param>
        /// <returns></returns>
        IOnboardingI9DocumentQuery ByEmployeeId(int employeeId);

        /// <summary>
        /// Filters document by category.
        /// </summary>
        /// <param name="category">Category type.</param>
        /// <returns></returns>
        IOnboardingI9DocumentQuery ByCategory(string category);
    }
}

