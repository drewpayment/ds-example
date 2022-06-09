using System.Collections;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Core
{
    public class EmployeeAttachmentFolderDto
    {
        public int    EmployeeFolderId { get; set; }
        public int?   EmployeeId       { get; set; }
        public int?   ClientId         { get; set; }
        public string Description      { get; set; } 
        public bool   IsEmployeeView   { get; set; } 
        public bool   IsAdminViewOnly  { get; set; } 
        public bool? IsDefaultOnboardingFolder { get; set; }
        public bool? IsDefaultPerformanceFolder { get; set; }
    }

    public class EmployeeAttachmentFolderDetailDto : EmployeeAttachmentFolderDto
    {
        public int AttachmentCount { get; set; }
        public bool IsSystemFolder { get; set; }
        public bool IsCompanyFolder { get; set; }
        public bool ShowAttachments { get; set; }
        public bool? IsDefaultOnboardingFolder { get; set; }
        public bool? DefaultATFolder { get; set; }
        public IEnumerable<EmployeeAttachmentDto> Attachments { get; set; }
    }
}
