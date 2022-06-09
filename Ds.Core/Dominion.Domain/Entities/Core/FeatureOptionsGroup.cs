using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Core
{
    public class FeatureOptionsGroup : Entity<FeatureOptionsGroup>
    {
        public virtual int FeatureOptionsGroupId { get; set; }
        public virtual string Description { get; set; }

        public virtual IEnumerable<AccountFeatureInfo> FeatureOptions { get; set; }
    }
}
