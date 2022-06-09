using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Core
{
    /// <summary>
    /// Query on <see cref="EmployeeAttachment"/>(s).
    /// </summary>
    public interface IEmployeeAttachmentQuery : IQuery<EmployeeAttachment, IEmployeeAttachmentQuery>
    {
        /// <summary>
        /// Filters by one or more distinct employee attachment(s).
        /// </summary>
        /// <param name="attachmentIds">ID(s) of the employee attachment(s) to query.</param>
        /// <returns></returns>
        IEmployeeAttachmentQuery ByAttachementId(params int[] attachmentIds);

        /// <summary>
        /// Filters by attachments for a single client.
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        IEmployeeAttachmentQuery ByClientId(int clientId);  

        /// <summary>
        /// Filters by attachments belonging to a single employee. 
        /// </summary>
        /// <param name="employeeId">ID of employee to get attachments for.</param>
        /// <param name="includeSharedAttachments">If true (default), documents belonging to multiple employees 
        /// will be included in the query results.</param>
        /// <returns></returns>
        IEmployeeAttachmentQuery ByEmployeeId(int employeeId, bool includeSharedAttachments = true);

        /// <summary>
        /// Filters by attachments contained in a single folder.
        /// </summary>
        /// <param name="folderId">ID of the folder to get attachments for.</param>
        /// <returns></returns>
        IEmployeeAttachmentQuery ByFolderId(int folderId);

        /// <summary>
        /// Filters out any attachments that the employee cannot view (i.e. admin-only files).
        /// </summary>
        /// <returns></returns>
        IEmployeeAttachmentQuery IncludeEmployeeViewableOnly();

        IEmployeeAttachmentQuery ByOnboardingWorkFlowTaskId(int onboardingWorkFlowTaskId);

        IEmployeeAttachmentQuery ByIsDeleted(bool isDeleted);
    }
}
