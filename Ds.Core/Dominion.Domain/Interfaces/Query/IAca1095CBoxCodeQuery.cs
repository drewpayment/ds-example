using Dominion.Domain.Entities.Aca;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IAca1095CBoxCodeTypeQuery : IQuery<Aca1095CBoxCodeTypeInfo, IAca1095CBoxCodeTypeQuery>
    {
        /// <summary>
        /// Filters by a box code's active status.
        /// </summary>
        /// <param name="isActive">Includes box codes that meet the specified activity status.</param>
        /// <returns></returns>
        IAca1095CBoxCodeTypeQuery ByIsActive(bool isActive = true);
    }
}
