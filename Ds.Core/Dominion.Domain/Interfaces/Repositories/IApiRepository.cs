using Dominion.Domain.Entities.Api;
using Dominion.Domain.Interfaces.Query;

namespace Dominion.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Provides access to Api Related Entities
    /// </summary>
    public interface IApiRepository
    {
        /// <summary>
        /// returns a query of <see cref="ApiAccountMapping"/> data
        /// </summary>
        /// <returns></returns>
        IApiAccountMappingQuery GetApiAccountMappingQuery();

        /// <summary>
        /// returns Queries <see cref="ApiAccount"/> data
        /// </summary>
        /// <returns></returns>
        IApiAccountQuery GetApiAccountQuery();

        /// <summary>
        /// Returns Api entities
        /// </summary>
        /// <returns></returns>
        IApiQuery GetApiQuery();

        /// <summary>
        /// returns ApiVersion entities
        /// </summary>
        /// <returns></returns>
        IApiVersionQuery GetApiVersionQuery();


    }
}