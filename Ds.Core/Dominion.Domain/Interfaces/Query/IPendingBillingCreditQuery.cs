using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Billing;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IPendingBillingCreditQuery : IQuery<PendingBillingCredit, IPendingBillingCreditQuery>
    {
        IPendingBillingCreditQuery ByClientId(int clientId);
        IPendingBillingCreditQuery ByNeedsApproval();
    }
}
