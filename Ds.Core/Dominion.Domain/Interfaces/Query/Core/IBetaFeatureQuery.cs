using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query.Core
{
    public interface IBetaFeatureQuery : IQuery<BetaFeature, IBetaFeatureQuery>
    {
        IBetaFeatureQuery ByBetaFeatureId(int betaFeatureId);
    }
}
