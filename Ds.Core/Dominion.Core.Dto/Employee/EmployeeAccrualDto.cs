using Dominion.Core.Dto.Accruals;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    [Serializable]
    public class EmployeeAccrualDto
    {
        public int EmployeeAccrualId { get; set; }
        public int EmployeeId { get; set; }
        public int ClientAccrualId { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsAllowScheduledAwards { get; set; }
        public DateTime? WaitingPeriodDate { get; set; }
        public bool IsActive { get; set; }
        public ClientAccrualDto ClientAccrual { get; set; }

        public EmployeeAccrualDto(EmployeeAccrualInfoDto info)
        {
            this.ClientAccrualId = info.ClientAccrualId;
            this.EmployeeId = info.EmployeeId;
            this.EmployeeAccrualId = 0;
            this.IsAllowScheduledAwards = info.AllowScheduledAwards;
            this.WaitingPeriodDate = null;
            this.IsActive = info.IsActive;
            this.ClientAccrual = null;
        }
        public EmployeeAccrualDto()
        {

        }
    }
}
