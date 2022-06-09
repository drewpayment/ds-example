using Dominion.Domain.Entities.Aca;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IAca1095CBoxCodeOptionQuery : IQuery<Aca1095CBoxCodeOption, IAca1095CBoxCodeOptionQuery>
    {
        /// <summary>
        /// Filters by a box code's active status.
        /// </summary>
        /// <param name="isActive">Includes box codes that meet the specified activity status.</param>
        /// <returns></returns>

        /// <summary>
        /// Filters by a box code's active status.
        /// </summary>
        /// <param name="isActive">Includes box codes that meet the specified activity status.</param>
        /// <returns></returns>
        IAca1095CBoxCodeOptionQuery ByYear(short year);
    }
}
