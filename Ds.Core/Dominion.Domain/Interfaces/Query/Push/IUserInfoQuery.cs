using Dominion.Domain.Entities.Push;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Push 
{
    public interface IUserInfoQuery : IQuery<PushUserInfo, IUserInfoQuery>
    {
        /// <summary>
        /// Filters PushUserInfo entities by device serial number
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        IUserInfoQuery BySerialNumber(string serialNumber);

        /// <summary>
        /// Filters PushUserInfo entities by device serial numbers
        /// </summary>
        /// <param name="serialNumbers"></param>
        /// <returns></returns>
        IUserInfoQuery BySerialNumbers(string[] serialNumbers);
    }
}
