using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Security;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository providing query access to various security objects.
    /// </summary>
    public interface ISecurityRepository : IRepository, IDisposable
    {
        /// <summary>
        /// Provides a way to query application-wide security settings that meet the specified query criteria.
        /// </summary>
        /// <typeparam name="TResult">Type of object to return.</typeparam>
        /// <param name="query">Filter/selection criteria to apply to the result set.</param>
        /// <returns></returns>
        IEnumerable<TResult> GetSecuritySettings<TResult>(QueryBuilder<SecuritySettingInfo, TResult> query)
            where TResult : class;

        /// <summary>
        /// Queries <see cref="UserSupervisorSecuritySetting"/>(s).
        /// </summary>
        /// <returns></returns>
        IUserSupervisorSecuritySettingQuery QueryUserSupervisorSecuritySettings();
    }
}