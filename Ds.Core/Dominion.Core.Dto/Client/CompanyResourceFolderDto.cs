using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class CompanyResourceFolderDto
    {
        public int CompanyResourceFolderId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public bool IsNew { get; set; }
        public int ResourceCount { get; set; }

        public IEnumerable<CompanyResourceDto> ResourceList { get; set; }
        public int NewResourceId { get; set; }
    }
}
