using Dominion.Domain.Entities.App;
using Dominion.Domain.Interfaces.Query.App;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface IApplicationConfigurationRepository
    {
        /// <summary>
        /// Queries on <see cref="ApplicationResource"/> data.
        /// </summary>
        /// <returns></returns>
        IApplicationResourceQuery QueryApplicationResources();

        /// <summary>
        /// Queries on <see cref="ApplicationMenu"/> data.
        /// </summary>
        /// <returns></returns>
        IApplicationMenuQuery QueryApplicationMenus();

        /// <summary>
        /// Queries on <see cref="ApplicationMenuItem"/> data.
        /// </summary>
        /// <returns></returns>
        IApplicationMenuItemQuery QueryApplicationMenuItems();
    }
}
