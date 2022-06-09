using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;
using System;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientAccountFeatureQuery : IQuery<ClientAccountFeature, IClientAccountFeatureQuery>
    {
        IClientAccountFeatureQuery ByClientId(int clientId);
        IClientAccountFeatureQuery ByAccountFeatureId(AccountFeatureEnum accountFeature);
    }
}
