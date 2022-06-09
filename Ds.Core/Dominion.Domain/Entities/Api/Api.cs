using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;

namespace Dominion.Domain.Entities.Api
{
    public partial class Api : Entity<Api>
    {
        public virtual int ApiId { get; set; }
        public virtual string BaseUrl { get; set; }
        public virtual string Name { get; set; }
        public virtual string CustomerHeaderKey { get; set; }
        public virtual AccountFeatureEnum? FeatureOptionId { get; set; }

        //REVERSE NAVIGATION

		public virtual AccountFeatureInfo AccountFeatureInfo { get; set; } 
        public virtual ICollection<ApiAccount> ApiAccount { get; set; } // many-to-one;
        public virtual ICollection<ApiVersion> ApiVersion { get; set; } // many-to-one;

    }
}
