using Dominion.Core.Dto.App;
using Dominion.Domain.Entities.App;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.App
{
    public interface IApplicationResourceQuery : IQuery<ApplicationResource, IApplicationResourceQuery>
    {
        IApplicationResourceQuery ByResourceId(int resourceId);

        IApplicationResourceQuery ByApplicationSource(ApplicationSourceType source);

        IApplicationResourceQuery ByResourceType(ApplicationResourceType resourceType);
    }
}
