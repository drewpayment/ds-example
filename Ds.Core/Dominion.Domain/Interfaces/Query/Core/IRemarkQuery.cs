using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Core
{
    public interface IRemarkQuery : IQuery<Remark, IRemarkQuery>
    {
        /// <summary>
        /// Filters entities by remark id.
        /// </summary>
        /// <param name="remarkId"></param>
        /// <returns></returns>
        IRemarkQuery ByRemark(int remarkId);

        IRemarkQuery ByGoal(int goalId);
    }
}
