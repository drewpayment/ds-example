using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Api;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientDepartment : Entity<ClientDepartment>, IHasModifiedData
    {
        public virtual int ClientDepartmentId { get; set; }
        public virtual int ClientDivisionId { get; set; }
        public virtual int? DepartmentHeadEmployeeId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Code { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual int ClientId { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }

        // FOREIGN KEYS
        public virtual Employee.Employee DepartmentHeadEmployee { get; set; }
        public virtual ClientDivision Division { get; set; }
        public virtual ICollection<Employee.Employee> Employees { get; set; }
        public virtual ICollection<ApiAccountMapping> ApiAccountMapping { get; set; } // many-to-one;
        public virtual ICollection<ClientGLAssignment> ClientGLAssignments { get; set; }

    }
}