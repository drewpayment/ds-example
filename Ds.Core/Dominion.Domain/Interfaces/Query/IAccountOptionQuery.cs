using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Misc;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Query on <see cref="AccountOptionInfo"/> data.
    /// </summary>
    public interface IAccountOptionQuery : IQuery<AccountOptionInfo, IAccountOptionQuery>
    {
        /// <summary>
        /// Filters by a particular account option.
        /// </summary>
        /// <param name="option">Option to query.</param>
        /// <returns></returns>
        IAccountOptionQuery ByAccountOption(AccountOption option);

        IAccountOptionQuery ByCategory(AccountOptionCategory category);

        /// <summary>
        /// Filters by a particular clientn.
        /// </summary>
        /// <param name="clientId">Option to query.</param>
        /// <returns></returns>
        IAccountOptionQuery ByClient(int clientId);
    }
}
