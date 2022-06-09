using System.Collections.Generic;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Api
{
    public partial class ApiVersion : Entity<ApiVersion>
    {
        public virtual int ApiVersionId { get; set; }
        public virtual int ApiId { get; set; }
        public virtual string Version { get; set; }

        //REVERSE NAVIGATION
        public virtual ICollection<ApiAccount> ApiAccount { get; set; } // many-to-one;

        //FOREIGN KEYS
        public virtual Api Api { get; set; }
    }
}