using Dominion.Domain.Entities.App;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.App
{
    public interface IApplicationMenuItemQuery : IQuery<ApplicationMenuItem, IApplicationMenuItemQuery>
    {
        IApplicationMenuItemQuery ByMenuItemId(int menuItemId);
        IApplicationMenuItemQuery ByMenuId(int menuId);

        IApplicationMenuItemQuery ByHasParentId(bool hasParentId);
    }
}