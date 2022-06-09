using System;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    /// <summary>
    /// Entity representation of a dbo.RequestTimeOffDetail record.
    /// </summary>
    public partial class TimeOffRequestDetail : Entity<TimeOffRequestDetail>, IHasModifiedData
    {
        public virtual int       TimeOffRequestDetailId { get; set; } 
        public virtual int       TimeOffRequestId       { get; set; } 
        public virtual DateTime  RequestDate            { get; set; } 
        public virtual double    Hours                  { get; set; } 
        public virtual int       ModifiedBy             { get; set; } 
        public virtual DateTime  Modified               { get; set; } 
        public virtual DateTime? FromTime               { get; set; } 
        public virtual DateTime? ToTime                 { get; set; } 
        public virtual int       ClientId               { get; set; } 
        public virtual int       EmployeeId             { get; set; } 

        public virtual Employee.Employee Employee       { get; set; }
        public virtual Client            Client         { get; set; }
        public virtual TimeOffRequest    TimeOffRequest { get; set; }
    }
}