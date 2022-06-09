using Dominion.Domain.Entities.Tax.Legacy;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Constructs a query on <see cref="LegacyDisabilityTax"/> entities.
    /// </summary>
    public interface ILegacyDisabilityTaxQuery : IQuery<LegacyDisabilityTax, ILegacyDisabilityTaxQuery>
    {
        ILegacyDisabilityTaxQuery ByDisabilityTaxIds(IEnumerable<int?> taxIds);

        ILegacyDisabilityTaxQuery ByStateId(int stateId);
    }
}
