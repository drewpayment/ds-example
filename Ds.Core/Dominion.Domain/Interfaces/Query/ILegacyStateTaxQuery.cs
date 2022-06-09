using Dominion.Domain.Entities.Tax.Legacy;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Represents a query on <see cref="LegacyStateTax"/> entities.
    /// </summary>
    public interface ILegacyStateTaxQuery : IQuery<LegacyStateTax, ILegacyStateTaxQuery>
    {
        /// <summary>
        /// Filters the results by taxes containing the specified description.
        /// </summary>
        /// <param name="desc">Case-insensitive text the tax's description must contain.</param>
        /// <returns></returns>
        ILegacyStateTaxQuery ByDescription(string desc);

        /// <summary>
        /// Filters the results by taxes with the specified ID.
        /// </summary>
        /// <param name="id">ID of the state tax to filter by.</param>
        /// <returns></returns>
        ILegacyStateTaxQuery ByStateTaxId(int id);

        /// <summary>
        /// Filters the results by taxes for the given state.
        /// </summary>
        /// <param name="stateId">ID of the state to filter state taxes by.</param>
        /// <returns></returns>
        ILegacyStateTaxQuery ByStateId(int stateId);

        /// <summary>
        /// Filters state taxes which have a valid State FUTA tax definition.
        /// </summary>
        /// <returns></returns>
        ILegacyStateTaxQuery ByIsStateFutaDefined();

        ILegacyStateTaxQuery ByStateTaxIds(IEnumerable<int?> stateTaxIds);
    }
}
