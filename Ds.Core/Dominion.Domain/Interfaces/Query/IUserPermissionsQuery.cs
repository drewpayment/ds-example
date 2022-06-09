using Dominion.Domain.Entities.User;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IUserPermissionsQuery : IQuery<UserPermissions, IUserPermissionsQuery>
    {
        /// <summary>
        /// Filters by a specific user id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IUserPermissionsQuery ByUserId(int userId);
    }
}
