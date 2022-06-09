using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.LeaveManagement
{
    public class LeaveManagementPendingAwardDto
    {
        public int LeaveManagementPendingAwardId { get; set; }
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
        public int ClientAccrualId { get; set; }
        public int ClientEarningId { get; set; }

        public double Amount { get; set; }
        public DateTime AwardDate { get; set; }
        public Byte AmountType { get; set; }
        public Byte AwardType { get; set; }
        public int? PayrollAdjustmentId { get; set; }
    }
}
