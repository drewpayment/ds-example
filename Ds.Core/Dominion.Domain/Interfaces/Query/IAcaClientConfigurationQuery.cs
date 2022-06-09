using System.Collections.Generic;
using Dominion.Domain.Entities.Aca;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Queries <see cref="AcaClientConfiguration"/> data.
    /// </summary>
    public interface IAcaClientConfigurationQuery : IQuery<AcaClientConfiguration, IAcaClientConfigurationQuery>
    {
        /// <summary>
        /// Filters configuration data by the specified client.
        /// </summary>
        /// <param name="clientId">ID of client to filter configuration data for.</param>
        /// <returns></returns>
        IAcaClientConfigurationQuery ByClientId(int clientId);

        /// <summary>
        /// Filters configuration data by the specified ACA reporting year.
        /// </summary>
        /// <param name="year">ACA reporting year.</param>
        /// <returns></returns>
        IAcaClientConfigurationQuery ByYear(int year);

        /// <summary>
        /// Filters configuration data by the specified ACA reporting year and clientIds.
        /// </summary>
        /// <param name="year">ACA reporting year.</param>
        /// <param name="clientIds"></param>
        /// <returns></returns>
        IAcaClientConfigurationQuery ByYearByClintIds(int year, IEnumerable<int> clientIds);

    }
}
