using Dominion.Domain.Entities.App;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.App
{
    public interface IApplicationMenuQuery : IQuery<ApplicationMenu, IApplicationMenuQuery>
    {
        IApplicationMenuQuery ByMenuId(int menuId);
    }
}
