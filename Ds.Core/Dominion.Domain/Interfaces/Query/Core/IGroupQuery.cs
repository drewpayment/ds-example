using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query.Core
{
    public interface IGroupQuery : IQuery<Group, IGroupQuery>
    {

        IGroupQuery ByClient(int clientId);
        IGroupQuery ByGroupId(int groupId);
        IGroupQuery ByClientIdList(IEnumerable<int> clientIds);
        IGroupQuery ByHasReviewTemplateGroup();
    }
}
