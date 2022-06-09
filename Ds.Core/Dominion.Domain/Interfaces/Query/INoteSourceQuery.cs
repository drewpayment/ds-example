using Dominion.Domain.Entities.Misc;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface INoteSourceQuery : IQuery<NoteSourceEntity, INoteSourceQuery>
    {
        INoteSourceQuery NoFilter();
    }
}
