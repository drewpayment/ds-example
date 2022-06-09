using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;

namespace Dominion.Domain.Entities.Employee
{
    public class EmployeePoints : Entity<EmployeePoints>
    {
        public virtual int EmployeePointsId { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual int? EmployeePointsCodeId { get; set; }
        public virtual double Hours { get; set; }
        public virtual double Amount { get; set; }
        public virtual DateTime? ExpireDate{ get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual DateTime ModifiedDate{ get; set; }
        public virtual string Comments{ get; set; }
        public virtual DateTime DateOccured { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int? ClockEmployeePointHistoryAdjustmentId { get; set; }
        public virtual bool IsAutoPoint { get; set; }
        public virtual int? ClockClientPointExceptionId { get; set; }
        public virtual int? ClockEmployeeExceptionHistoryId { get; set; }
        public virtual int? ClockClientPerfectAttendanceId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Client Client { get; set; }
    }
}