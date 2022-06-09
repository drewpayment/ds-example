using Dominion.Core.Dto.Billing;
using Dominion.Domain.Entities.Billing;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IBillingItemDescriptionQuery : IQuery<BillingItemDescription, IBillingItemDescriptionQuery>
    {
        IBillingItemDescriptionQuery ByBillingItemDescriptionId(BillingItemDescriptionType billingItemDesc);

        IBillingItemDescriptionQuery ByCode(string code);

        IBillingItemDescriptionQuery ByIsActive(bool isActive = true);
    }
}
