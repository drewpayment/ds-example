using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Tax.Legacy;
using Dominion.Domain.Interfaces.Query;

namespace Dominion.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository providing query access to Legacy Tax entities (eg: <see cref="LegacyStateTax"/>, 
    /// <see cref="LegacyLocalTax"/>, etc).
    /// </summary>
    public interface ILegacyTaxRepository : IRepository, IDisposable
    {
        /// <summary>
        /// Creates a new <see cref="LegacyStateTax"/> query.
        /// </summary>
        /// <returns><see cref="LegacyStateTax"/> query.</returns>
        ILegacyStateTaxQuery LegacyStateTaxQuery();

        /// <summary>
        /// Creates a new <see cref="LegacyLocalTax"/> query.
        /// </summary>
        /// <returns><see cref="LegacyLocalTax"/> query.</returns>
        ILegacyLocalTaxQuery LegacyLocalTaxQuery();

        /// <summary>
        /// Creates a new <see cref="LegacyDisabilityTax"/> query.
        /// </summary>
        /// <returns><see cref="LegacyDisabilityTax"/> query.</returns>
        ILegacyDisabilityTaxQuery LegacyDisabilityTaxQuery();

        /// <summary>
        /// Creates a new <see cref="LegacyFederalTaxProfileQuery"/> query.
        /// </summary>
        /// <returns><see cref="LegacyFederalTaxProfileQuery"/> query.</returns>
        ILegacyFederalTaxProfileQuery LegacyFederalTaxProfileQuery();
    }
}
