using System.Collections.Generic;
using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Api;
using Dominion.Domain.Entities.Core;

namespace Dominion.Domain.Entities.Misc
{
    /// <summary>
    /// Account Feature Entity providing additional information about the AccountFeature enumeration
    /// </summary>
    public class AccountFeatureInfo : Entity<AccountFeatureInfo>
    {
        // Basic Properties
        public virtual AccountFeatureEnum AccountFeatureId { get; set; }
        public virtual string Description { get; set; }
        public virtual int FeatureOptionsGroupId { get; set; }
        public virtual FeatureOptionsGroup FeatureOptionGroup { get; set; }
        public virtual bool IsClientEnabled { get; set; }
        public virtual ICollection<Api.Api> Api { get; set; } // many-to-one;
    }
}
