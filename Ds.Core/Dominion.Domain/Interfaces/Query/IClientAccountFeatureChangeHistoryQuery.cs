using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;
using System;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientAccountFeatureChangeHistoryQuery : IQuery<ClientAccountFeatureChangeHistory, IClientAccountFeatureChangeHistoryQuery>
    {
        IClientAccountFeatureChangeHistoryQuery ByClientId(int clientId);
        IClientAccountFeatureChangeHistoryQuery ByAccountFeatureId(AccountFeatureEnum accountFeature);
        IClientAccountFeatureChangeHistoryQuery OrderByDate();
        IClientAccountFeatureChangeHistoryQuery ByAfterEndDate(DateTime periodEndDate);
    }
}
