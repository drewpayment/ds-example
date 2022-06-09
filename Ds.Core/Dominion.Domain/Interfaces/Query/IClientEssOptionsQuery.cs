
using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
     /// <summary>
    /// Query on <see cref="ClientEssOptions"/>.
    /// </summary>
    
    public interface IClientEssOptionsQuery  : IQuery<ClientEssOptions, IClientEssOptionsQuery>
    {
        IClientEssOptionsQuery ByClientId(int clientId);

        IClientEssOptionsQuery ByAllowImageUpload();
    }
}