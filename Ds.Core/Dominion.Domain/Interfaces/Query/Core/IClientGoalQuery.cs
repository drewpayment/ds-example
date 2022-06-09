using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Core
{
    public interface IClientGoalQuery : IQuery<ClientGoal, IClientGoalQuery>
    {
        /// <summary>
        /// Filters entities by client id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClientGoalQuery ByClient(int clientId);

        /// <summary>
        /// Filters entities by goal id.
        /// </summary>
        /// <param name="goalId"></param>
        /// <returns></returns>
        IClientGoalQuery ByGoal(int goalId);
        IClientGoalQuery ByEmployee(int employeeId);
        IClientGoalQuery ByIncludeInReview(bool includeInReview = true);
        IClientGoalQuery ByIsArchived(bool isArchived);
    }
}
