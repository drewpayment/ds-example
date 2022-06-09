using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class ReviewProfileBasicSetupDto : ReviewProfileBasicDto
    {
        public string DefaultInstructions { get; set; }
        public bool IncludeReviewMeeting { get; set; }
        public bool IncludeScoring { get; set; }
        public bool IncludePayrollRequests { get; set; }
    }
}
