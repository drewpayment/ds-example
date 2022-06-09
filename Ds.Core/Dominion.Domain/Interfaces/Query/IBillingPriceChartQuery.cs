using Dominion.Core.Dto.Billing;
using Dominion.Domain.Entities.Billing;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Queries a <see cref="BillingPriceChart"/> datasource.
    /// </summary>
    public interface IBillingPriceChartQuery : IQuery<BillingPriceChart, IBillingPriceChartQuery>
    {
        /// <summary>
        /// Filters by the specified price charts. If no types are specified, ALL charts will be included.
        /// </summary>
        /// <param name="chartTypes">Price chart types to include.</param>
        /// <returns></returns>
        IBillingPriceChartQuery ByPriceChartTypes(params BillingPriceChartType[] chartTypes);
    }
}
