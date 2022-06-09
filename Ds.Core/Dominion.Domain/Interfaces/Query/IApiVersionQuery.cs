using Dominion.Domain.Entities.Api;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApiVersionQuery : IQuery<ApiVersion, IApiVersionQuery>
    {
        IApiVersionQuery ByApiId(int apiId);
    }
}