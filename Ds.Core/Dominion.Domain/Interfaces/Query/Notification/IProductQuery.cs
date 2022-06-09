using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Notification
{
    public interface IProductQuery : IQuery<ProductInfo, IProductQuery>
    {
        /// <summary>
        /// Filters Products by product ID.
        /// </summary>
        /// <param name="productId">The ID of the product to filter products by.</param>
        /// <returns></returns>
        IProductQuery ByProductId(Product productId);

        /// <summary>
        /// Filters Products by whether they have notification types or not.
        /// </summary>
        /// <param name="hasNotificationTypes">Flag to filter products based on whether they have associated notification types.</param>
        /// <returns></returns>
        IProductQuery ByHasNotificationTypes(bool hasNotificationTypes = true);

        /// <summary>
        /// Filters out the Dominion Only Product.
        /// </summary>
        /// <returns></returns>
        IProductQuery ByIsNotDominionOnly();
    }
}
