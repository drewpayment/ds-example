using System.Collections.Generic;
using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientSubTopicQuery : IQuery<ClientSubTopic, IClientSubTopicQuery>
    {
        IClientSubTopicQuery ByClientTopicId(int clientTopicId);

        IClientSubTopicQuery ByClientTopicIds(params int[] clientTopicIds);

        IClientSubTopicQuery ByClientSubTopicId(int clientSubTopicId);

        IClientSubTopicQuery ByClientSubTopicIds(int clientId, params int[] clientSubTopicId);
    }
}