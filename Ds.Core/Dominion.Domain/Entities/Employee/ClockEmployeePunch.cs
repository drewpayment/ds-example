using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Employee.ClockEmployeeInfo;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Employee
{
    public partial class ClockEmployeePunch : Entity<ClockEmployeePunch>, IHasModifiedData
    {
        public virtual int ClockEmployeePunchId { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual DateTime ModifiedPunch { get; set; }
        public virtual DateTime RawPunch { get; set; }
        public virtual int? ClientCostCenterId { get; set; }
        public virtual int? ClientDivisionId { get; set; }
        public virtual int? ClientDepartmentId { get; set; }
        public virtual int? ClientShiftId { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual int RawPunchBy { get; set; }
        public virtual int? ClockEmployeePunchTypeId { get; set; }
        public virtual int? ClockClientLunchId { get; set; }
        public virtual bool IsPaid { get; set; }
        public virtual string Comment { get; set; }
        public virtual DateTime? ShiftDate { get; set; }
        public virtual bool? IsManualShiftOverride { get; set; }
        public virtual byte? TimeZoneId { get; set; }
        public virtual string ClockName { get; set; }
        public virtual byte? TransferOption { get; set; }
        public virtual bool IsStopAutoLunch { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int? ClockClientHardwareId { get; set; }
        public virtual int? ClockEmployeeScheduleId { get; set; }
        public virtual byte? ScheduleNumber { get; set; }
        public virtual int? ClientJobCostingAssignmentId1 { get; set; }
        public virtual int? ClientJobCostingAssignmentId2 { get; set; }
        public virtual int? ClientJobCostingAssignmentId3 { get; set; }
        public virtual int? ClientJobCostingAssignmentId4 { get; set; }
        public virtual int? ClientJobCostingAssignmentId5 { get; set; }
        public virtual int? ClientJobCostingAssignmentId6 { get; set; }
        public virtual string EmployeeComment { get; set; }
        public virtual int? ClockEmployeePunchLocationID { get; set; }

        //Reference Entities
        public virtual ClientCostCenter CostCenter { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ClockClientLunch Lunch { get; set; }
        public virtual ClockEmployeePunchLocation ClockEmployeePunchLocation { get; set; }

        public ClockEmployeePunch()
        {
        }
    }
}
