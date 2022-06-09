using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Core
{
    public interface IClientAzureQuery : IQuery<ClientAzure, IClientAzureQuery>
    {
        IClientAzureQuery ByClient(int clientId);
    }
}
