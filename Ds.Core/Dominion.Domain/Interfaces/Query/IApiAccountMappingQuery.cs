using Dominion.Domain.Entities.Api;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Querys the <see cref="ApiAccountMapping"/> datasource
    /// </summary>
    public interface IApiAccountMappingQuery : IQuery<ApiAccountMapping, IApiAccountMappingQuery>
    {

        /// <summary>
        /// Filters by the ApiAccountMappingId 
        /// </summary>
        /// <param name="apiAccountMappingId"></param>
        /// <returns></returns>
        IApiAccountMappingQuery ByApiAccountMappingId(int apiAccountMappingId);

        /// <summary>
        /// Filters by ApiAccountId.  If querying mappings, filter by this id before calling one of the
        /// genereic mapping filters
        /// </summary>
        /// <param name="apiAccountId"></param>
        /// <returns></returns>
        IApiAccountMappingQuery ByApiAccountId(int apiAccountId);

        /// <summary>
        /// Filters by ClientDepartmentId
        /// </summary>
        /// <param name="clientDepartmentId"></param>
        /// <returns></returns>
        IApiAccountMappingQuery ByClientDepartmentId(int clientDepartmentId);

        /// <summary>
        /// Filters by ClientDivisionId
        /// </summary>
        /// <param name="clientDivsionId"></param>
        /// <returns></returns>
        IApiAccountMappingQuery ByClientDivisionId(int clientDivsionId);

        /// <summary>
        /// Filters by EmployeeId
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        IApiAccountMappingQuery ByEmployeeId(int employeeId);

        /// <summary>
        /// Filters by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IApiAccountMappingQuery ByUserId(int userId);

        /// <summary>
        /// Filters by JobProfileId
        /// </summary>
        /// <param name="jobProfileId"></param>
        /// <returns></returns>
        IApiAccountMappingQuery ByJobProfileId(int jobProfileId);

        /// <summary>
        /// returns all items where ClientDepartmentId is not null
        /// </summary>
        /// <returns></returns>
        IApiAccountMappingQuery GetClientDepartmentMappings();

        /// <summary>
        /// returns all items where ClientDivisionId is not null
        /// </summary>
        /// <returns></returns>
        IApiAccountMappingQuery GetClientDivisionMappings();

        /// <summary>
        /// returns all items where JobProfileId is not null
        /// </summary>
        /// <returns></returns>
        IApiAccountMappingQuery GetJobProfilesMappings();

        /// <summary>
        /// returns all items where UserId is not null
        /// </summary>
        /// <returns></returns>
        IApiAccountMappingQuery GetUserMappings();

        /// <summary>
        /// returns all items where EmployeeId is not null
        /// </summary>
        /// <returns></returns>
        IApiAccountMappingQuery GetEmployeeMappings();
    }
}