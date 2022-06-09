using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.Security;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientCostCenter : Entity<ClientCostCenter>
    {
        public virtual int       ClientCostCenterId    { get; set; }
        public virtual int       ClientId              { get; set; }
        public virtual string    Code                  { get; set; }
        public virtual string    Description           { get; set; }
        public virtual int?      DefaultGlAccountId    { get; set; }
        public virtual bool?     IsDefaultGlCostCenter { get; set; }
        public virtual DateTime? Modified              { get; set; }
        public virtual string    ModifiedBy            { get; set; }
        public virtual string    GlClassName           { get; set; }
        public virtual bool      IsActive              { get; set; }
        public virtual bool      IsExcludeFromGLFile   { get; set; }

        public virtual ICollection<Employee.Employee>                 Employees                     { get; set; }
        public virtual ICollection<ScheduleGroup>                     ScheduleGroups                { get; set; }
        public virtual Client                                         Client                        { get; set; }
        public virtual ICollection<UserSupervisorSecurityGroupAccess> UserSupervisorSecurities      { get; set; }
        public virtual ICollection<ClientCostCenterRatePremium>       RatePremiumInfo               { get; set; }
        public virtual ICollection<ClientGLAssignment> ClientGLAssignments { get; set; }

    }
}