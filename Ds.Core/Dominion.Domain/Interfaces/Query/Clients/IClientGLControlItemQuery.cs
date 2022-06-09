using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Interfaces.Query.Clients
{
    public interface IClientGLControlItemQuery : IQuery<ClientGLControlItem, IClientGLControlItemQuery>
    {
        IClientGLControlItemQuery ByClientId(int clientId);
        IClientGLControlItemQuery ExcludeNonAssignedItems();
    }
}
