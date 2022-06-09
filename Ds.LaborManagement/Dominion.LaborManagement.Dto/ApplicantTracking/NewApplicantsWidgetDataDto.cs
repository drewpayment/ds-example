using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class NewApplicantsWidgetDataDto
    {
        public IEnumerable<NewApplicantDataPointDto> DataPoints { get; set; }
        public int TotalNewApplicantsLastWeek { get; set; }
    }
}
