using Dominion.Core.Dto.Performance.ReviewTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{

    public class MultiReviewDialogResultDto
    {
        public ReviewWithEmployeesDto CalendarYear { get; set; }
        public ReviewTemplateBaseDto DateOfHire { get; set; }
        public int? Owner { get; set; }
        public int Supervisor { get; set; }
    }
}
