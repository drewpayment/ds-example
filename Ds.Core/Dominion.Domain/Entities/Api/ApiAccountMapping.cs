using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Api
{
    public partial class ApiAccountMapping : Entity<ApiAccountMapping>, IHasModifiedOptionalData, IHasOptionalSyncData
    {
        public virtual int ApiAccountMappingId { get; set; }
        public virtual int? ApiAccountId { get; set; }
        public virtual int? ExternalId { get; set; }
        public virtual int? ClientDepartmentId { get; set; }
        public virtual int? ClientDivisionId { get; set; }
        public virtual int? UserId { get; set; }
        public virtual int? EmployeeId { get; set; }
        public virtual int? JobProfileId { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual DateTime? LastSync { get; set; }
        public virtual DateTime? LastSyncAttempt { get; set; }

        //FOREIGN KEYS
        public virtual ApiAccount ApiAccount { get; set; }
        public virtual ClientDepartment ClientDepartment { get; set; }
        public virtual ClientDivision ClientDivision { get; set; }
        public virtual User.User User { get; set; }
        public virtual Employee.Employee Employee { get; set; }
        public virtual JobProfile JobProfile { get; set; }
    }
}