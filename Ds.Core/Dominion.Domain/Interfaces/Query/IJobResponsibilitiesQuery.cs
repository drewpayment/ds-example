using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IJobResponsibilitiesQuery : IQuery<JobResponsibilities, IJobResponsibilitiesQuery>
    {
        IJobResponsibilitiesQuery ByJobResponsibilityId(int jobResponsibilityId);
        IJobResponsibilitiesQuery ByClientId(int clientId);
    }
}
