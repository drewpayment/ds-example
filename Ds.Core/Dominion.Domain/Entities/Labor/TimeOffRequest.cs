using System;
using System.Collections.Generic;
using Dominion.Core.Dto.LeaveManagement;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    /// <summary>
    /// Entity representation of a dbo.RequestTimeOff record.
    /// </summary>
    public partial class TimeOffRequest : Entity<TimeOffRequest>, IHasModifiedData
    {
        public virtual int               TimeOffRequestId    { get; set; } 
        public virtual int               ClientAccrualId     { get; set; } 
        public virtual int               EmployeeId          { get; set; } 
        public virtual DateTime          RequestFrom         { get; set; } 
        public virtual DateTime          RequestUntil        { get; set; } 
        public virtual double?           Hours               { get; set; } 
        public virtual string            RequesterNotes      { get; set; } 
        public virtual string            ApproverNotes       { get; set; } 
        public virtual TimeOffStatusType Status              { get; set; } 
        public virtual DateTime          Modified            { get; set; } 
        public virtual int               ModifiedBy          { get; set; } 
        public virtual double?           BalanceBeforeApp    { get; set; } 
        public virtual double?           BalanceAfterApp     { get; set; } 
        public virtual DateTime?         OriginalRequestDate { get; set; } 
        public virtual int               ClientId            { get; set; } 

        public virtual ClientAccrual        ClientAccrual { get; set; }
        public virtual Client               Client        { get; set; }
        public virtual Employee.Employee    Employee      { get; set; }
        public virtual TimeOffRequestStatusInfo StatusInfo    { get; set; }

        public virtual ICollection<TimeOffRequestDetail> TimeOffRequestDetails { get; set; } 
    }
}