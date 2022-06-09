using System.Collections.Generic;
using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.User;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IUserClientQuery : IQuery<UserClient, IUserClientQuery>
    {
        /// <summary>
        /// Filters by users associated with the specified client (per UserClient).
        /// </summary>
        /// <param name="clientId">ID of client the user is associated with.</param>
        /// <returns></returns>
        IUserClientQuery ByClientId(int clientId);
        IUserClientQuery ByUserId(int userId);

        /// <summary>
        /// Filters by users who are benefit administrators.
        /// </summary>
        /// <returns></returns>
        IUserClientQuery ByIsBenefitAdministrator(bool isAdmin = true);

        /// <summary>
        /// Filters by users with the specified 'disabled' setting.
        /// </summary>
        /// <param name="isDisabled">If true, will return only users who are disabled. If false, will return active users.</param>
        /// <returns></returns>
        IUserClientQuery ByUserIsDisabled(bool isDisabled);
        IUserClientQuery ByUserType(UserType userType);

        /// <summary>
        /// Filters by user types included in the given list of user types.
        /// </summary>
        /// <param name="userTypes">The list of user types to include in the filter.</param>
        /// <returns></returns>
        IUserClientQuery ByUserTypes(List<UserType> userTypes);

        /// <summary>
        /// Filters out user types found in the given list of user types.
        /// </summary>
        /// <param name="userTypes">The list of user types to exclude in the filter.</param>
        /// <returns></returns>
        IUserClientQuery ByExcludeUserTypes(List<UserType> userTypes);

    }
}
