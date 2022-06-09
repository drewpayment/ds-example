using Dominion.Domain.Entities.Banks;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Banks
{
    public interface IAchBankQuery : IQuery<AchBank, IAchBankQuery>
    {
        IAchBankQuery ByIsTaxManagement(bool isTaxManagement = true);

        IAchBankQuery ByAchBankId(int achBankId);
    }
}
