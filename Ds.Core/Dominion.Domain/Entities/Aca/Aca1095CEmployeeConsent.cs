using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// ACA employee electronic consent info. (Entity for [dbo].[Employee1095CConsent] table)
    /// </summary>
    public partial class Aca1095CEmployeeConsent : Entity<Aca1095CEmployeeConsent>, IHasModifiedData
    {
        public virtual int               EmployeeId          { get; set; }
        public virtual int               ClientId            { get; set; }
        public virtual DateTime?         ConsentDate         { get; set; }
        public virtual DateTime?         WithdrawalDate      { get; set; }
        public virtual string            PrimaryEmailAddress { get; set; }
        public virtual bool              IsEmailVerified     { get; set; }
        public virtual DateTime          Modified            { get; set; }
        public virtual int               ModifiedBy          { get; set; }
        public virtual Employee.Employee Employee            { get; set; }
        public virtual Client            Client              { get; set; }
    }
}
