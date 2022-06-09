using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Core
{
    public partial class ResourceDto
    {
        public int ResourceId { get; set; }
        public int? ClientId { get; set; }
        public int? EmployeeId { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public ResourceSourceType SourceTypeId { get; set; }
        public string Source { get; set; }
        public DateTime AddedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
        public int? AddedBy { get; set; }

        // RELATIONSHIPS

        public virtual ImageResourceDto ImageResource { get; set; }
        public virtual AzureResourceDto AzureResource { get; set; }
    }
}
