using Dominion.Domain.Interfaces.Query.Push;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface IPushRepository
    {
        IMachineQuery QueryMachines();

        IUserInfoQuery QueryUserInfo();

        ITemplateQuery QueryTemplates();

        ITransactionQuery QueryTransactions();
        IMachineToClockClientHardwareQuery QueryUnassignedMachines();
    }
}