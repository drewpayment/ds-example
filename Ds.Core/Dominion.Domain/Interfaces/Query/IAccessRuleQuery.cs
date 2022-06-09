using System.Collections.Generic;
using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.User;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IAccessRuleQuery : IQuery<AccessRule, IAccessRuleQuery>
    {
        /// <summary>
        /// Filters access rules by those associated with item detail.
        /// </summary>
        /// <param name="hasItemDetail">If true (default), will only return access rules with associated
        /// with item detail.</param>
        /// <param name="sources">If specified, will only return access rules associated with the specified
        /// rule sources.</param>
        /// <returns></returns>
        IAccessRuleQuery ByHasItemDetail(bool hasItemDetail = true, IEnumerable<ClaimSource> sources = null);
    }
}
