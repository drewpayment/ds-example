using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantSkillQuery : IQuery<ApplicantSkill, IApplicantSkillQuery>
    {
        IApplicantSkillQuery ByApplicantId(int applicantId);
        IApplicantSkillQuery ByEnabled(bool isEnabled);
        IApplicantSkillQuery ByApplicantSkillId(int id);
    }
}
