using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Core
{
    public interface ITaskQuery : IQuery<Task, ITaskQuery>
    {
        ITaskQuery ByTask(int taskId);
        ITaskQuery ByTaskIds(ICollection<int> taskIds);

        ITaskQuery ByParent(int parentId);
    }
}
