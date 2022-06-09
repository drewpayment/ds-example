using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.User;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IUserActionTypeQuery : IQuery<UserActionType, IUserActionTypeQuery>
    {
        /// <summary>
        /// Filters by action types allowed by default for the given user type.
        /// </summary>
        /// <param name="userType"></param>
        /// <returns></returns>
        IUserActionTypeQuery ByLegacyUserType(UserType userType);
    }
}
