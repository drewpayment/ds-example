using Dominion.Domain.Interfaces.Query.ClientFeaturesInfo;
using System;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface IClientNotesRepository : IRepository, IDisposable
    {
        IClientNotesQuery QueryClientNotes();
    }
}
