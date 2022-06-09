using Dominion.Domain.Entities.Accounting;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientGLInterfaceQuery : IQuery<ClientGLInterface, IClientGLInterfaceQuery>
    {
        IClientGLInterfaceQuery ByClientId(int clientId);
    }
}
