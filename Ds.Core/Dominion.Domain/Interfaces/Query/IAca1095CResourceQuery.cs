using Dominion.Domain.Entities.Aca;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Constructs a query on <see cref="Aca1095CResource"/>(s).
    /// </summary>
    public interface IAca1095CResourceQuery : IQuery<Aca1095CResource, IAca1095CResourceQuery>
    {
        /// <summary>
        /// Filters by a single resource.
        /// </summary>
        /// <param name="resourceId">ID of resource.</param>
        /// <returns></returns>
        IAca1095CResourceQuery ByResourceId(int resourceId);
    }
}
