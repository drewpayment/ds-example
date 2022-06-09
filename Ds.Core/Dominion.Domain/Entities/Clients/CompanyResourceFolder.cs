using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Clients
{
    public class CompanyResourceFolder : Entity<CompanyResourceFolder>
    {
        public virtual int CompanyResourceFolderId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Description { get; set; }
        public virtual ICollection<CompanyResource> CompanyResource { get; set; }
    }
}
