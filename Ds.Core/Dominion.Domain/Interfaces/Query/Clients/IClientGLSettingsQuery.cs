using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query.Clients
{
    public interface IClientGLSettingsQuery : IQuery<ClientGLSettings, IClientGLSettingsQuery>
    {
        IClientGLSettingsQuery ByClientId(int clientId);
    }
}
