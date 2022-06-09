using Dominion.Domain.Interfaces.Query;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface IAccountingRepository
    {
        IGeneralLedgerAccountQuery QueryGeneralLedgerAccounts();
        IClientGLInterfaceQuery QueryClientGLInterfaces();
    }
}