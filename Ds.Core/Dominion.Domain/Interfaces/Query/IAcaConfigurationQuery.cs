using System.Collections.Generic;
using Dominion.Domain.Entities.Aca;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IAcaConfigurationQuery : IQuery<AcaConfiguration, IAcaConfigurationQuery>
    {
        /// <summary>
        /// Filters 1095C data by employee.
        /// </summary>
        /// <param name="configurationId">Employee to filter by.</param>
        /// <returns>Query to be further filtered.</returns>
        IAcaConfigurationQuery ByConfigurationId(int configurationId);

        /// <summary>
        /// Fitlers by a given ACA tax year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        IAcaConfigurationQuery ByYear(short year);

        /// <summary>
        /// Filters by the active ACA tax year (i.e. <see cref="AcaConfiguration.IsPriorYear"/> is false)
        /// </summary>
        /// <returns></returns>
        IAcaConfigurationQuery ByCurrentYear();

        /// <summary>
        /// Filters by only configurations for years that are marked as 'Production' ready.
        /// </summary>
        /// <returns></returns>
        IAcaConfigurationQuery ByProductionStatusOnly();
    }
}