using System.Security.Permissions;
using Dominion.Core.Dto.Client;

namespace Dominion.Core.Dto.Core
{
    public class ResourceAccessInfoDto : IResourceSourceIdentifiers
    {
        public int                            ResourceId                         { get; set; }
        public ResourceSourceType             SourceType                         { get; set; }
        public string                         Source                             { get; set; }
        public string                         ResourceName                       { get; set; }
        public int?                           ClientId                           { get; set; }
        public int?                           EmployeeId                         { get; set; }
        public bool                           IsAttachment                       { get; set; }
        public bool?                          IsAttachmentEmployeeViewable       { get; set; }
        public int?                           AttachementFolderId                { get; set; }
        public bool?                          IsAttachmentFolderEmployeeViewable { get; set; }
        public bool?                          IsAttachmentFolderAdminViewOnly    { get; set; }
        public bool                           IsCompanyResource                  { get; set; }
        public bool?                          IsManagerLink                      { get; set; }
        public bool?                          IsAzure                            { get; set; }
        public int?                           AzureAccount                       { get; set; }
        public CompanyResourceSecurityLevel?  ResourceSecurityLevel              { get; set; }
    }
}
