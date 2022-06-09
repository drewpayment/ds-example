using Dominion.Core.Dto.User;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query.App
{
    public interface IAccessRuleQuery : IQuery<AccessRule, IAccessRuleQuery>
    {
        IAccessRuleQuery ByHasItemDetail(bool hasItemDetail = true, IEnumerable<ClaimSource> sources = null);
    }
}
