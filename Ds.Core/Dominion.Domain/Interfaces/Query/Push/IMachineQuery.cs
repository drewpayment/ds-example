using Dominion.Domain.Entities.Push;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Push 
{
    public interface IMachineQuery : IQuery<PushMachine, IMachineQuery>
    {
        IMachineQuery BySerialNumber(string serialNumber);
    }
}
