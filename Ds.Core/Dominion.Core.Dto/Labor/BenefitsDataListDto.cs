using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public class BenefitsDataListDto
    {
        public bool IsBenefitPortalOn { get; set; }
        public IEnumerable<BenefitPackagesDto> BenefitPackageList { get; set; }
        public IEnumerable<SalaryDeterminationMethodDto> SalaryDeterminationMethodList { get; set; }
    }
}
