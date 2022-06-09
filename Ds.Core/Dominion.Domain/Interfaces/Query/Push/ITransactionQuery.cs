using Dominion.Domain.Entities.Push;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Push
{
    public interface ITransactionQuery : IQuery<PushAttLog, ITransactionQuery>
    {
        /// <summary>
        /// Filters PushUserInfo entities by device serial number
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        ITransactionQuery BySerialNumber(string serialNumber);

        /// <summary>
        /// Filters PushUserInfo entities by device serial numbers
        /// </summary>
        /// <param name="serialNumbers"></param>
        /// <returns></returns>
        ITransactionQuery BySerialNumbers(string[] serialNumbers);
    }
}
