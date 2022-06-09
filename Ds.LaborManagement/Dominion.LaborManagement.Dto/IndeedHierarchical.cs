using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply
{
    public class IndeedHierarchical : IndeedAnswers
    {
        public IEnumerable<IndeedStandard> HierarchicalAnswers { get; set; }
        public IEnumerable<string> Values { get; set; }
    }
}
