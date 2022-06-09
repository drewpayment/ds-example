using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Clients
{
    /// <summary>
    /// EF entity for dbo.ClientGroup table.
    /// </summary>
    public class ClientGroup : Entity<ClientGroup>
    {
        public virtual int      ClientGroupId { get; set; }
        public virtual int      ClientId      { get; set; }
        public virtual string   Code          { get; set; }
        public virtual string   Description   { get; set; }
        public virtual DateTime Modified      { get; set; }
        public virtual string   ModifiedBy    { get; set; }

        // relationships
        public virtual Client   Client        { get; set; }
        public virtual ICollection<Employee.Employee> Employees { get; set; }
        public virtual ICollection<ClientGLAssignment> ClientGLAssignments { get; set; }
    }
}
