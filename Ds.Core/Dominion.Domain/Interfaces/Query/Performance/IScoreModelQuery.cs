using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface IScoreModelQuery : IQuery<ScoreModel, IScoreModelQuery>
    {
        IScoreModelQuery ByClient(int clientId);
        IScoreModelQuery ByScoreModelId(int scoreModelId);

        IScoreModelQuery ByActive(bool isActive = true);
    }
}
