using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class PayrollRequestDto
    {
        public int ReviewTemplateId { get; set; }
        public string ReviewTemplateName { get; set; }
        public int MeritIncreaseCount { get; set; }
        public int OneTimeCount { get; set; }
        public int ApprovedMeritIncreaseCount { get; set; }
        public int ApprovedAdditionalEarningCount { get; set; }
        public int DeclinedMeritIncreaseCount { get; set; }
        public int DeclinedAdditionalEarningCount { get; set; }
        public int PendingMeritIncreaseCount { get; set; }
        public int PendingAdditionalEarningCount { get; set; }
        public ICollection<ReviewDto> Reviews { get; set; }
        public ICollection<PayrollRequestItemsDto> PayrollRequestItems { get; set; }  
    }
}
