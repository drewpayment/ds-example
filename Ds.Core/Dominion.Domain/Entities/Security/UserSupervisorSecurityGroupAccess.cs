using Dominion.Core.Dto.Security;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;

namespace Dominion.Domain.Entities.Security
{
    /// <summary>
    /// Gets the supervisor security access information for specific types.
    /// See: UserSecurityGroupType enum (search for it)
    /// This is where we say 'supervisor a' has access to 'client cost center a'.
    /// The table is a lookup style table with no primary key or foreign keys. 
    /// View source: dbo.UserSupervisorSecurity
    /// </summary>
    public class UserSupervisorSecurityGroupAccess : Entity<UserSupervisorSecurityGroupAccess>
    {
        public virtual int                      UserSupervisorSecurityId               { get; set; }
        public virtual int                      UserId                                 { get; set; }
        public virtual int                      ClientId                               { get; set; }
        public virtual int                      ForeignKeyId                           { get; set; }
        public virtual UserSecurityGroupType    UserSecurityGroupId                    { get; set; }

        /// <summary>
        /// THIS IS READ ONLY. 
        /// It's a computed column that will only have a value if this is security for client cost center and will get it's value from the ForeignKeyId.
        /// This is a computed column which should be fine since this entity should never be CREATING client cost centers.
        /// </summary>
        public virtual int?                     ClientCostCenterId                     { get; set; }



        /// <summary>
        /// THIS IS READ ONLY. 
        /// It's a computed column that will only have a value if this is security for client cost center and will get it's value from the ForeignKeyId.
        /// This is a computed column which should be fine since this entity should never be CREATING client cost centers.
        /// </summary>
        public virtual ClientCostCenter ClientCostCenter { get; set; }

        public virtual User.User User { get; set; }
    }
}
