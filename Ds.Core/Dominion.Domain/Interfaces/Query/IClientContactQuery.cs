using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface  IClientContactQuery : IQuery<ClientContact, IClientContactQuery>
    {
        IClientContactQuery ByClientId(int clientId);
        IClientContactQuery ByClientIdActive(int clientId);
        IClientContactQuery ByClientContactId(int clientContactId);
        IClientContactQuery ByClientContactIds(List<int> clientIds);
        IClientContactQuery ByClientContactIds(int[] clientIds);
        IClientContactQuery ByClientIds(IEnumerable<int> clientIds);
        IClientContactQuery ByIsPrimary(bool pm);
        
    }
}
