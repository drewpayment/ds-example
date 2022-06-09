using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Core
{
    public interface IGoalQuery : IQuery<Goal, IGoalQuery>
    {
        IGoalQuery ByGoal(int goalId);

        IGoalQuery ByEmployee(int employeeId);

        IGoalQuery ByClient(int clientId);

        /// <summary>
        /// Filters a list of goals by the assigned to field. 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IGoalQuery ByOwner(int userId);
		
		IGoalQuery ByArchived();
    }
}
