using System.Collections.Generic;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IJobProfileClassificationsQuery : IQuery<JobProfileClassifications, IJobProfileClassificationsQuery>
    {
        IJobProfileClassificationsQuery ByJobProfileId(int jobProfileId);
    }
}
