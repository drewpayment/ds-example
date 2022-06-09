using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Api
{
    public partial class ApiAccount : Entity<ApiAccount>, IHasModifiedOptionalData
    {
        public virtual int ApiAccountId { get; set; }
        public virtual int ApiId { get; set; }
        public virtual int? ApiVersionId { get; set; }
        public virtual string ApiKey { get; set; }
        public virtual int? AuthType { get; set; }
        public virtual string Subdomain { get; set; }

        //FOREIGN KEYS
        public virtual Api Api { get; set; }
        public virtual ApiVersion ApiVersion { get; set; }

        // many-to-many ClientApiAccount.FK_ClientApiAccount_Client;
        public virtual ICollection<ClientApiAccount> ClientApiAccount { get; set; }
        public virtual ICollection<ApiAccountMapping> ApiAccountMapping { get; set; } // many-to-one;


        public virtual int? ModifiedBy { get; set; }
        public virtual DateTime? Modified { get; set; }
    }
}