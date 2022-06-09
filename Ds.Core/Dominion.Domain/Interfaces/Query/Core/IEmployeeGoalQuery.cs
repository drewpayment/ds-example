using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;
using Dominion.Core.Dto.Core;
using System.Linq.Expressions;

namespace Dominion.Domain.Interfaces.Query.Core
{
    public interface IEmployeeGoalQuery : IQuery<EmployeeGoal, IEmployeeGoalQuery>
    {
        /// <summary>
        /// Filter entities by employee id.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        IEmployeeGoalQuery ByEmployee(int employeeId);

        /// <summary>
        /// Filter entities by goal id.
        /// </summary>
        /// <param name="goalId"></param>
        /// <returns></returns>
        IEmployeeGoalQuery ByGoal(int goalId);

        /// <summary>
        /// Filters by goals that should be part of a review.
        /// </summary>
        /// <param name="includeInReview"></param>
        /// <returns></returns>
        IEmployeeGoalQuery ByIncludeInReview(bool includeInReview = true);
        IEmployeeGoalQuery ByGoalsWithCompanyAlignment();
        IEmployeeGoalQuery ByGoalsWithParentEmployeeGoal();
        IEmployeeGoalQuery ByIsArchived(bool isArchived);
    }
}
