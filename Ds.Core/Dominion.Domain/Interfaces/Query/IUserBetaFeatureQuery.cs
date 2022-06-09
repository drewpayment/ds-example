using Dominion.Domain.Entities.User;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IUserBetaFeatureQuery : IQuery<UserBetaFeature, IUserBetaFeatureQuery>
    {
        IUserBetaFeatureQuery ByUserId(int userId);
        IUserBetaFeatureQuery ByBetaFeatureId(int betaFeatureId);
    }
}
