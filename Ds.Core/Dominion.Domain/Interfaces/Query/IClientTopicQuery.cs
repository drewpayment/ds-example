using System.Collections.Generic;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Misc;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientTopicQuery : IQuery<ClientTopic, IClientTopicQuery>
    {
        IClientTopicQuery ByClientId(int clientId);
        IClientTopicQuery ByClientTopicId(params int[] clientTopicIds);
    }
}