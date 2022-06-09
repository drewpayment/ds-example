using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Clients
{
    public partial class ClientGLCustomClass : Entity<ClientGLCustomClass>
    {
        public virtual int ClientGLCustomClassId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Description { get; set; }
        public virtual int ClientDivisionId { get; set; }
        public virtual int ClientGroupId { get; set; }
        public virtual int ClientDepartmentId { get; set; }
        public virtual int ClientCostCenterId { get; set; }

        // Foreign Keys
        public virtual ClientDivision   ClientDivision   { get; set; }
        public virtual ClientGroup      ClientGroup      { get; set; }
        public virtual ClientDepartment ClientDepartment { get; set; }
        public virtual ClientCostCenter ClientCostCenter { get; set; }
        public virtual Client           Client           { get; set; }
    }
}
