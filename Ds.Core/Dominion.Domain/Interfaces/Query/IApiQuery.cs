using Dominion.Domain.Entities.Api;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApiQuery : IQuery<Api, IApiQuery>
    {
        IApiQuery ByApiId(int apiId);

        IApiQuery WithAccountFeature();
    }
}