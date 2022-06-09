using Dominion.Core.Dto.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee.Search
{
    public class ReviewTemplateSearchDto
    {
        public int? DelayAfterReference { get; set; }
        public DateUnit? DelayAfterReferenceUnitTypeId { get; set; }
        public int? ReviewTemplateId { get; set; }
    }
}
