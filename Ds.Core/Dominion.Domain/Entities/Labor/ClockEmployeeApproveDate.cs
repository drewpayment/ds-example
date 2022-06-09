using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public partial class ClockEmployeeApproveDate : Entity<ClockEmployeeApproveDate>, IHasModifiedOptionalData
    {
        public virtual int       ClockEmployeeApproveDateId { get; set; } 
        public virtual int       EmployeeId                 { get; set; } 
        public virtual DateTime  EventDate                  { get; set; } 
        public virtual bool?     IsApproved                 { get; set; } 
        public virtual int?      PayrollId                  { get; set; } 
        public virtual int?      ClientCostCenterId         { get; set; } 
        public virtual int?      ClientEarningId            { get; set; } 
        public virtual int?      ClockClientNoteId          { get; set; } 
        public virtual bool      IsPayToSchedule            { get; set; } 
        public virtual int       ClientId                   { get; set; } 
        public virtual int?      ApprovedBy                 { get; set; } 
        public virtual DateTime? ApprovedDate               { get; set; } 
        public virtual int?      ModifiedBy                 { get; set; } 
        public virtual DateTime? Modified                   { get; set; } 

        public virtual Client            Client   { get; set; }
        public virtual Employee.Employee Employee { get; set; }
        public virtual Payroll.Payroll   Payroll  { get; set; }
    }
}
