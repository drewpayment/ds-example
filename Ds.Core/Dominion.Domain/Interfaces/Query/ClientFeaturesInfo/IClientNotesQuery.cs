using Dominion.Domain.Entities.ClientFeatures;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.ClientFeaturesInfo
{
    public interface IClientNotesQuery : IQuery<ClientNotes, IClientNotesQuery>
    {
        IClientNotesQuery ByClientId(int clientId);
    }
}
