using System.Collections.Generic;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Core
{
    /// <summary>
    /// Constructs a query on <see cref="Resource"/> data.
    /// </summary>
    public interface IResourceQuery : IQuery<Resource, IResourceQuery>
    {
        /// <summary>
        /// Filters by a single resource.
        /// </summary>
        /// <param name="resourceId">ID of resource to filter by.</param>
        /// <returns></returns>
        IResourceQuery ByResource(int resourceId);

        /// <summary>
        /// Filters by a single resource.
        /// </summary>
        /// <param name="resourceId">ID of resource to filter by.</param>
        /// <returns></returns>
        IResourceQuery ByResources(IEnumerable<int> resourceIds);

        /// <summary>
        /// Filters resources for a single employee.
        /// </summary>
        /// <param name="employeeId"> ID of employee to filter by.</param> 
        /// <returns></returns>
        IResourceQuery ByEmployee(int employeeId);

        /// <summary>
        /// Filters resources for an enumerable of employees.
        /// </summary>
        /// <param name="employeeIds"> IDs of employees to filter by.</param> 
        /// <returns></returns>
        IResourceQuery ByEmployees(IEnumerable<int> employeeIds);

        /// <summary>
        /// Filters by a single resource.
        /// </summary>
        /// <param name="sourceTypeId">ID of resource type to filter by.</param>
        /// <returns></returns>
        IResourceQuery BySourceType(ResourceSourceType sourceTypeId);

        /// <summary>
        /// Filters out any resources marked 'deleted'.
        /// </summary>
        /// <returns></returns>
        IResourceQuery ByNotDeleted();

        /// <summary>
        /// Filters resources by whether or not they are deleted.
        /// </summary>
        /// <param name="isDeleted">True, returns only deleted resources.</param>
        /// <returns></returns>
        IResourceQuery ByIsDeleted(bool isDeleted);

        /// <summary>
        /// Filters resource by client id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IResourceQuery ByClient(int clientId);

        /// <summary>
        /// Filters resources by related image resource image type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IResourceQuery ByImageType(ImageType type);

        /// <summary>
        /// Fitlers resources by related image resource image size. 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        IResourceQuery ByImageSizeType(ImageSizeType size);

        /// <summary>
        /// Filters resources by entities that only have a corresponding AzureResource entity.
        /// </summary>
        /// <returns></returns>
        IResourceQuery HasAzureResource();

        /// <summary>
        /// Fitlers resources by its AzureResource dependent entity's name property. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IResourceQuery ByAzureResourceName(string name);
    }
}
