using Dominion.Core.Dto.Billing;
using Dominion.Domain.Entities.Billing;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IAutomaticBillingQuery : IQuery<AutomaticBilling, IAutomaticBillingQuery>
    {
        IAutomaticBillingQuery ByFeatureOptionId(int featureOptionId);
        IAutomaticBillingQuery ByBillingItemDescriptionId(BillingItemDescriptionType billingItemDescriptionId);
    }
}
