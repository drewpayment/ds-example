using Dominion.Domain.Entities.Employee.Search;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Employee.Search
{
    public interface ILegacyEmployeeSearchSettingQuery : IQuery<LegacyEmployeeSearchSetting, ILegacyEmployeeSearchSettingQuery>
    {
        /// <summary>
        /// Filters search settings for a single user.
        /// </summary>
        /// <param name="userId">ID of user to query search settings for.</param>
        /// <returns></returns>
        ILegacyEmployeeSearchSettingQuery ByUserId(int userId);
    }
}
