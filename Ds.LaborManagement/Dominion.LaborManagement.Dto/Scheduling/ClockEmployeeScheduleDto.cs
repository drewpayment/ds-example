using System;

namespace Dominion.LaborManagement.Dto.Scheduling
{
    public class ClockEmployeeScheduleDto
    {
        public virtual int ClockEmployeeScheduleId { get; set; }
        public virtual DateTime EventDate { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int? ClockClientTimePolicyId { get; set; }

        public virtual DateTime? StartTime1 { get; set; }
        public virtual DateTime? EndTime1 { get; set; }
        public virtual int? ClientDepartmentId1 { get; set; }
        public virtual int? ClientCostCenterId1 { get; set; }

        public virtual DateTime? StartTime2 { get; set; }
        public virtual DateTime? EndTime2 { get; set; }
        public virtual int? ClientDepartmentId2 { get; set; }
        public virtual int? ClientCostCenterId2 { get; set; }

        public virtual DateTime? StartTime3 { get; set; }
        public virtual DateTime? EndTime3 { get; set; }
        public virtual int? ClientDepartmentId3 { get; set; }
        public virtual int? ClientCostCenterId3 { get; set; }
    }
}