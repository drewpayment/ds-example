using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockClientNotesQuery : IQuery<ClockClientNote, IClockClientNotesQuery>
    {
        IClockClientNotesQuery ByClient(int clientId);

        IClockClientNotesQuery ByClockClientNoteId(int clockClientNoteId);
        IClockClientNotesQuery ByIsActive(bool isActive);
    }
}