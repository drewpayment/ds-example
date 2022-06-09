using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Clients
{
    public interface IClientFeatureTrackingQuery : IQuery<ClientFeatureTracking,IClientFeatureTrackingQuery>
    {
        IClientFeatureTrackingQuery ByClientId(int clientId);
        IClientFeatureTrackingQuery ByClientIds(IEnumerable<int> clientIds);
        IClientFeatureTrackingQuery ByFeatureOptionId(AccountFeatureEnum featureOption);
        IClientFeatureTrackingQuery ByIsEnabled(bool isEnabled = true);
        IClientFeatureTrackingQuery ByModified(DateTime modified);
    }
}
