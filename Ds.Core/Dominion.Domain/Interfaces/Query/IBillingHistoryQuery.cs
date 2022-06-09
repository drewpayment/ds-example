using System;
using Dominion.Domain.Entities.Billing;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IBillingHistoryQuery : IQuery<BillingHistory, IBillingHistoryQuery>
    {
        IBillingHistoryQuery ByClientId(int clientId);
        IBillingHistoryQuery ByGenPayrollHistoryId(int genPayrollHistoryId);
        IBillingHistoryQuery ByIsPaid(bool isPaid = true);
        IBillingHistoryQuery ByCheckDateOnOrAfterDate(DateTime dt);
        IBillingHistoryQuery ByGenBillingHistoryId(int genBillingHistoryId);
    }
}
