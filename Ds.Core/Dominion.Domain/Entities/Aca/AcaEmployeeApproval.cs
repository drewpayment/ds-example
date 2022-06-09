using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;
namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// ACA per-employee approval status for a specified reporting year. (Entity for [dbo].[EmployeeAcaApproval] table)
    /// </summary>
    public partial class AcaEmployeeApproval : Entity<AcaEmployeeApproval>, IHasModifiedData
    {
        public virtual int       EmployeeId           { get; set; }
        public virtual short     Year                 { get; set; } 
        public virtual int       ClientId             { get; set; }
        public virtual DateTime? ApprovalDate         { get; set; }
        public virtual DateTime? NotificationDateTime { get; set; }
        public virtual DateTime  Modified             { get; set; }
        public virtual int       ModifiedBy           { get; set; }

        //FOREIGN KEYS
        public virtual Employee.Employee Employee { get; set; }
        public virtual Client            Client { get; set; }
    }
}
