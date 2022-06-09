using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Core
{
    /// <summary>
    /// Query on <see cref="EmployeeAttachmentFolder"/> data.
    /// </summary>
    public interface IEmployeeAttachmentFolderQuery : IQuery<EmployeeAttachmentFolder, IEmployeeAttachmentFolderQuery>
    {
        /// <summary>
        /// Filters by folders belonging to one or more employee(s).
        /// </summary>
        /// <param name="includeGenericFolders">If true, will also include folders not associated with a particular employee.</param>
        /// <param name="employeeIds">Employee(s) to get folders for.</param>
        /// <returns></returns>
        IEmployeeAttachmentFolderQuery ByEmployeeId(bool includeGenericFolders, params int[] employeeIds);

        /// <summary>
        /// Filters by one or more specific folder(s).
        /// </summary>
        /// <param name="folderIds">Folder(s) to get.</param>
        /// <returns></returns>
        IEmployeeAttachmentFolderQuery ByFolderId(params int[] folderIds);

        /// <summary>
        /// Filters by folders owned (created) by a single user.
        /// </summary>
        /// <param name="userId">ID of the owner's user.</param>
        /// <returns></returns>
        IEmployeeAttachmentFolderQuery ByFolderOwner(int userId);

        /// <summary>
        /// Returns only system-admin generated folders.
        /// </summary>
        /// <returns></returns>
        IEmployeeAttachmentFolderQuery OnlySystemLevelFolders();

        /// <summary>
        /// Excludes system-admin generated folders.
        /// </summary>
        /// <returns></returns>
        IEmployeeAttachmentFolderQuery ExcludeSystemLevelFolders();

        /// <summary>
        /// Excludes company-wide folders.
        /// </summary>
        /// <returns></returns>
        IEmployeeAttachmentFolderQuery ExcludeCompanyLevelFolders();
        
        /// <summary>
        /// Excludes employee-specific folders.
        /// </summary>
        /// <returns></returns>
        IEmployeeAttachmentFolderQuery ExcludeEmployeeLevelFolders();

        /// <summary>
        /// Filters folders by only those viewable by employees.
        /// </summary>
        /// <returns></returns>
        IEmployeeAttachmentFolderQuery IncludeEmployeeViewableFoldersOnly();

        /// <summary>
        /// Filters by folders belonging to one or more client(s).
        /// </summary>
        /// <param name="includeGenericFolders">If true, will also include folders not associated with a particular client.</param>
        /// <param name="clientId">Client(s) to get folders for.</param>
        /// <returns></returns>
        IEmployeeAttachmentFolderQuery ByClientId(bool includeGenericFolders, params int[] clientId);

        /// <summary>
        /// Filters by folders with the specified group-level access. Also will include folders without any group-level
        /// access set.
        /// </summary>
        /// <param name="employeeId">If specified, folder must contain group-level access for the specified employee.</param>
        /// <param name="departmentId">If specified, folder must contain group-level access for the specified department.</param>
        /// <param name="costCenterId">If specified, folder must contain group-level access for the specified cost center.</param>
        /// <returns></returns>
        IEmployeeAttachmentFolderQuery IncludeFolderGroupAccess(int? employeeId = null, int? departmentId = null, int? costCenterId = null);

        /// <summary>
        /// Filters by folders used to store 'Onboarding' attachments.
        /// </summary>
        /// <returns></returns>
        IEmployeeAttachmentFolderQuery ByIsDefaultOnboardingFolder();

        IEmployeeAttachmentFolderQuery ExcludeDefaultOnboardingFolder();
        IEmployeeAttachmentFolderQuery ByIsDefaultATFolder();
		IEmployeeAttachmentFolderQuery ByIsDefaultPerformanceFolder();
    }
}
