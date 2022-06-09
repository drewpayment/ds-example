using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Entities.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Labor
{
    /// <summary>
    /// Entity representation of a dbo.LeaveManagementPendingAward record.
    /// </summary>
    public partial class LeaveManagementPendingAward : Entity<LeaveManagementPendingAward>
    {
        public virtual int LeaveManagementPendingAwardId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual int ClientAccrualId { get; set; }
        public virtual int ClientEarningId { get; set; }

        public virtual double Amount { get; set; }
        public virtual DateTime AwardDate { get; set; }
        public virtual Byte AmountType { get; set; }
        public virtual Byte AwardType { get; set; }
        public virtual int? PayrollAdjustmentId { get; set; }


        public virtual Client Client { get; set; }
        public virtual Employee.Employee Employee { get; set; }
        public virtual ClientAccrual ClientAccrual { get; set; }
        public virtual ClientEarning ClientEarning { get; set; }
    }
}
