using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.User
{
    public class UserBetaFeature : Entity<UserBetaFeature>, IHasModifiedData
    {
        public int UserBetaFeatureId { get; set; }
        public int UserId { get; set; }
        public int BetaFeatureId { get; set; }
        public bool IsBetaActive { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }

        // RELATIONSHIPS
        public virtual User User { get; set; }
        public virtual BetaFeature BetaFeature { get; set; }
    }
}
