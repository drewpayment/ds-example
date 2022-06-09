using System;

namespace Dominion.Core.Dto.Core
{
    [Serializable]
    public class EmployeeAttachmentDto
    {
        public int      ResourceId           { get; set; }
        public int      ClientId             { get; set; }
        public int?     EmployeeId           { get; set; }
        public string   Name                 { get; set; }
        public DateTime AddedDate            { get; set; }
        public string   AddedByUsername      { get; set; }
        public int?     FolderId             { get; set; }
        public bool     IsViewableByEmployee { get; set; }
        public bool     IsAzure              { get; set; }

        public ResourceSourceType SourceType { get; set; }
        public string   Extension            { get; set; }
        public string   Source               { get; set; }
        public string   cssClass             { get; set; }
        public int?     OnboardingWorkflowTaskId { get; set; }
        public bool 	IsATFile { get; set; } = false;
		public bool     IsCompanyAttachment      { get; set; }
        public bool IsDeleted { get; set; }

    }
}
