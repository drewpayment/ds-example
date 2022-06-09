using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Core;
using System.IO;

namespace Dominion.Core.Dto.Client
{
    public partial class CompanyResourceDto
    {
        public int? ClientId { get; set; }
        public int CompanyResourceFolderId { get; set; }
        public int ResourceId { get; set; }
        public string ResourceName { get; set; }
        public string ResourceFormat { get; set; }
        public CompanyResourceSecurityLevel SecurityLevel { get; set; }
        public ResourceSourceType ResourceTypeId { get; set; }
        public bool IsManagerLink { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public bool? DoesFileExist { get; set; }
        public bool? IsAzure { get; set; }
        public int? AzureAccount { get; set; }

        public string cssClass { get; set; }
        public string Source { get; set; }
        public string CurrentSource { get; set; }
        public bool IsNew { get; set; }
        public bool IsSelectedResource { get; set; }
        public bool PreviewResourceCssClass { get; set; }


        public DateTime AddedDate { get; set; }
        public int AddedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool isFileReselected { get; set; }

    }
}
