using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientDisciplineLevelQuery : IQuery<ClientDisciplineLevel, IClientDisciplineLevelQuery>
    {
        IClientDisciplineLevelQuery ByClientId(int clientId);
        IClientDisciplineLevelQuery ByDisciplineLevelId(int disciplineLevelId);
    }
}
