using Dominion.Domain.Entities.Api;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{

    /// <summary>
    /// Querys the <see cref="ApiAccount"/> datasource
    /// </summary>
    public interface IApiAccountQuery : IQuery<ApiAccount, IApiAccountQuery>
    {

        IApiAccountQuery ByApiAccountId(int apiAccountId);

        /// <summary>
        /// Filters by the specified clientId
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IApiAccountQuery ByClientId(int clientId);

        /// <summary>
        /// Filters by the specificed apiId
        /// </summary>
        /// <param name="apiId"></param>
        /// <returns></returns>
        IApiAccountQuery ByApiId(int apiId);
    }
}