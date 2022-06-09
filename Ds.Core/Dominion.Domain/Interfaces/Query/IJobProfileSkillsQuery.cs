using System.Collections.Generic;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IJobProfileSkillsQuery : IQuery<JobProfileSkills, IJobProfileSkillsQuery>
    {
        IJobProfileSkillsQuery ByJobProfileId(int jobProfileId);
        IJobProfileSkillsQuery ByJobSkillId(int jobSkillId);
    }
}
