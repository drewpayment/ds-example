using System.Collections.Generic;
using Dominion.Core.Dto.Tax;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Tax;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.Tax.Legacy;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Provides a way to query <see cref="ClientTaxDeferral"/> entities.
    /// </summary>
    public interface ITaxDeferralQuery : IQuery<ClientTaxDeferral, ITaxDeferralQuery>
    {
        ITaxDeferralQuery ByIsDeferred(bool isDeferred);
        ITaxDeferralQuery ByClientId(int clientId);
        ITaxDeferralQuery ByClientTaxDeferralId(int deferralId);
        ITaxDeferralQuery ByTaxDeferralType(TaxDeferralType type);
    }
}