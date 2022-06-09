using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.LeaveManagement
{
    public partial class ClientAccrualScheduleDto
    {
        public int ClientAccrualScheduleId { get; set; }
        public int ClientAccrualId { get; set; }
        public int ServiceStart { get; set; }
        public int ServiceStartFrequencyId { get; set; }
        public int ServiceFrequencyId { get; set; }
        public int? ServiceEnd { get; set; }
        public int? ServiceEndFrequencyId { get; set; }
        public double? AccrualBalanceLimit { get; set; }
        public double? BalanceLimit { get; set; }
        public double? Reward { get; set; }
        public int ServiceRewardFrequencyId { get; set; }
        public int? ServiceRenewFrequencyId { get; set; }
        public int? RenewEnd { get; set; }
        public double? CarryOver { get; set; }
        public int? ServiceCarryOverFrequencyId { get; set; }
        public int? ServiceCarryOverWhenFrequencyId { get; set; }
        public int? ServiceCarryOverTill { get; set; }
        public int? ServiceCarryOverTillFrequencyId { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }
        public double? RateCarryOverMax { get; set; }
    }
}
