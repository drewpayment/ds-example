using System.Collections.Generic;
using Dominion.Domain.Entities.User;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Query on <see cref="UserPin"/> data.
    /// </summary>
    public interface IUserPinQuery : IQuery<UserPin, IUserPinQuery>
    {
        /// <summary>
        /// Filter by a specific user PIN (by userPinId).
        /// </summary>
        /// <param name="userPinId">ID of the user PIN to filter by.</param>
        /// <returns></returns>
        IUserPinQuery ByUserPinId(int userPinId);

        /// <summary>
        /// Filter by a specific user PIN (by userId).
        /// </summary>
        /// <param name="userId">ID of the user to filter by.</param>
        /// <returns></returns>
        IUserPinQuery ByUserId(int userId);

        /// <summary>
        /// Filter by a specific user PIN (by clientContactId).
        /// </summary>
        /// <param name="clientContactId">ID of the contact to filter by.</param>
        /// <returns></returns>
        IUserPinQuery ByClientContactId(int clientContactId);

        /// <summary>
        /// Filter UserPins based on a list of client contact IDs.
        /// </summary>
        /// <param name="clientContactIds">List of IDs of client contacts to filter by.</param>
        /// <returns></returns>
        IUserPinQuery ByClientContactIds(List<int> clientContactIds);

        /// <summary>
        /// Filters by user PINs associated with the specified client (by clientId).
        /// </summary>
        /// <param name="clientId">ID of the client to filter by.</param>
        /// <returns></returns>
        IUserPinQuery ByClientId(int clientId);
    }
}
