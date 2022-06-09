using Dominion.Aca.Dto.Forms;

using Dominion.Domain.Entities.EEOC;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Query on <see cref="ClientEEOC"/>(s).
    /// </summary>
    public interface IClientEEOCQuery : IQuery<ClientEEOC, IClientEEOCQuery>
    {
        /// <summary>
        /// Filters the clientEEOC data for the given Client.
        /// </summary>
        /// <param name="CLientID">ID of the client.</param>
        /// <returns></returns>
        IClientEEOCQuery ByClientId(int clientId);

       
    }
}
