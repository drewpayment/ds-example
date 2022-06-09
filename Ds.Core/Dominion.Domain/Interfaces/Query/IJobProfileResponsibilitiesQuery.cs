using System.Collections.Generic;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IJobProfileResponsibilitiesQuery : IQuery<JobProfileResponsibilities, IJobProfileResponsibilitiesQuery>
    {
        IJobProfileResponsibilitiesQuery ByJobProfileId(int jobProfileId);
        IJobProfileResponsibilitiesQuery ByJobResponsibilityId(int jobResponsibilityId);
    }
}
