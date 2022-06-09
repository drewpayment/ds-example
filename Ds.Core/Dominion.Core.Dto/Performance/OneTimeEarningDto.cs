using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class OneTimeEarningDto
    {
        public int OneTimeEarningId { get; set; }
        public int EmployeeId { get; set; }
        public IncreaseType IncreaseType { get; set; }
        public double IncreaseAmount { get; set; }
        public DateTime MayBeIncludedInPayroll { get; set; }
        public ApprovalStatus ApprovalStatusID { get; set; }
        public int ClientEarningId { get; set; }
        public double ProposedTotalAmount { get; set; }
    }
}
