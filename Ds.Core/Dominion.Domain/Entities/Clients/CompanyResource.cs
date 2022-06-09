using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Client;
using Dominion.Domain.Entities.Onboarding;

namespace Dominion.Domain.Entities.Clients
{
    public class CompanyResource : Entity<CompanyResource>
    {
    
        public virtual int                          CompanyResourceFolderId { get; set; }
        public virtual int                          ResourceId { get; set; }
        public virtual CompanyResourceSecurityLevel SecurityLevel { get; set; }
        public virtual bool                         IsManagerLink { get; set; }
        public virtual DateTime                     Modified { get; set; }
        public virtual int                          ModifiedBy { get; set; }

        public virtual CompanyResourceFolder        CompanyResourceFolder { get; set; }
        public virtual Resource                     Resource { get; set; }
        public CompanyResource()
        {
        }
    }
}
