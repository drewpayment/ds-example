using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Core
{
    /// <summary>
    /// Entity representation of a [dbo].[EmployeeAttachment].
    /// </summary>
    public partial class EmployeeAttachment : Entity<EmployeeAttachment>
    {
        public virtual int      ResourceId           { get; set; }
        public virtual int?     EmployeeFolderId     { get; set; } 
        public virtual bool     IsEmployeeView       { get; set; }
        public virtual int?     OnboardingWorkflowTaskId { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual EmployeeAttachmentFolder Folder   { get; set; }
        public virtual Resource                 Resource { get; set; }
        public virtual  Onboarding.OnboardingWorkflowTask OnboardingWorkflowTask { get; set; }
    }
}
