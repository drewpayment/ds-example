using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IJobSkillsQuery : IQuery<JobSkills, IJobSkillsQuery>
    {
        IJobSkillsQuery ByJobSkillId(int jobSkillId);
        IJobSkillsQuery ByClientId(int clientId);
    }
}
