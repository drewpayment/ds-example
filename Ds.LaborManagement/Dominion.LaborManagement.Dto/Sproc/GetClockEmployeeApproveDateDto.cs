using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Sproc
{
    public class GetClockEmployeeApproveDateDto
    {
        public int EmployeeID { get; set; }
        public DateTime Eventdate { get; set; }
        public Boolean? IsApproved { get; set; }
        public int? ClientCostCenterID { get; set; }
        public int? ClientEarningID { get; set; }
        public Boolean? PayToSchedule { get; set; }
        public int? ClockClientNoteID { get; set; }
        public String Note { get; set; }
        public String ApprovingUser { get; set; }
        public int? DirectSupervisorId { get; set; }

    }

    public class ClockEmployeeApproveDateDto
    {
        public int ClockEmployeeApproveDateId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime EventDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
        public bool? IsApproved { get; set; }
        public int? PayrollId { get; set; }
        public int? ClientCostCenterId { get; set; }
        public int? ClientEarningId { get; set; }
        public int? ClockClientNoteId { get; set; }
        public bool IsPayToSchedule { get; set; }
        public int ClientId { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }

        //RELATIONSHIPS

        public virtual EmployeeBasicDto Employee { get; set; }
        public virtual ClientDto Client { get; set; }
        public virtual PayrollDto Payroll { get; set; }

        public ClockEmployeeApproveDateDto ShallowCopy()
        {
            return (ClockEmployeeApproveDateDto) this.MemberwiseClone();
        }
    }
}
