using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantSkillQuery : Query<ApplicantSkill, IApplicantSkillQuery>, IApplicantSkillQuery
    {
        public ApplicantSkillQuery(IEnumerable<ApplicantSkill> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        public IApplicantSkillQuery ByApplicantId(int applicantId)
        {
            FilterBy(x => x.ApplicantId == applicantId);
            return this;
        }
        public IApplicantSkillQuery ByEnabled(bool isEnabled)
        {
            FilterBy(x => x.IsEnabled == isEnabled);
            return this;
        }

        public IApplicantSkillQuery ByApplicantSkillId(int id)
        {
            FilterBy(x => x.ApplicantSkillId == id);
            return this;
        }
    }
}
