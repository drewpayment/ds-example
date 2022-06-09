using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dominion.Domain.Entities.Tax;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Constructs a query on <see cref="TaxOptionRule"/> entities.
    /// </summary>
    public interface ITaxOptionRuleQuery : IQuery<TaxOptionRule, ITaxOptionRuleQuery>
    {
        /// <summary>
        /// Filters the data set by rules belonging to the specified <see cref="TaxOption"/>.
        /// </summary>
        /// <param name="id">ID of the tax option the rule must belong to.</param>
        /// <returns></returns>
        ITaxOptionRuleQuery ByTaxOptionId(int id);

        /// <summary>
        /// Returns tax options matching the tax option ids passed in.
        /// </summary>
        /// <param name="taxOptionIds">Array of integers representing the tax option.</param>
        /// <returns></returns>
        ITaxOptionRuleQuery ByTaxOptionIds(IEnumerable<int> taxOptionIds);

    }
}
