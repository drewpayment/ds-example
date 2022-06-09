using Dominion.Domain.Interfaces.Query;
using Dominion.Domain.Interfaces.Query.Clients;

namespace Dominion.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repo methods for getting data for labor management.
    /// </summary>
    public interface IClientAccountFeatureRepository
    {
        IClientAccountFeatureQuery ClientAccountFeatureQuery();
        IClientAccountOptionQuery ClientOptionQuery();
        IAccountOptionItemQuery AccountOptionItemQuery();
        IFeatureOptionsGroupQuery FeatureOptionsGroupQuery();
        IClientFeatureTrackingQuery ClientFeatureTrackingQuery();
        IClientAccountFeatureChangeHistoryQuery ClientAccountFeatureChangeHistoryQuery();
    }
}
