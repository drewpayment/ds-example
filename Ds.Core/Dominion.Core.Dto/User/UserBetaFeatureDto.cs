using Dominion.Core.Dto.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.User
{
    public class UserBetaFeatureDto
    {
        public int UserBetaFeatureId { get; set; }
        public int UserId { get; set; }
        public int BetaFeatureId { get; set; }
        public bool IsBetaActive { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }

        public virtual UserDto User { get; set; }
        public virtual BetaFeatureDto BetaFeature { get; set; }
    }
}
