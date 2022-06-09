using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Api;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientDivision : Entity<ClientDivision>
    {
        public virtual int ClientDivisionId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int? ClientContactId { get; set; }
        public virtual string Name { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string City { get; set; }
        public virtual int StateId { get; set; }
        public virtual string Zip { get; set; }
        public virtual int CountryId { get; set; }
        public virtual byte SendCorrespondenceTo { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual bool? IsUseSeparateAccountRoutingNumber { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual int? BankId { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool IsUseAsStubAddress { get; set; }
        public virtual string LocationId { get; set; }
        public virtual DateTime? DateLocationOpened { get; set; }
        public virtual DateTime? DateLocationClosed { get; set; }


        // FOREIGN KEYS
        public virtual Client Client { get; set; }
        public virtual ICollection<Employee.Employee> Employees { get; set; }
        public virtual ICollection<ClientDepartment> Departments { get; set; }
        public virtual ICollection<ApiAccountMapping> ApiAccountMapping { get; set; } // many-to-one
        public virtual ICollection<ClientGLAssignment> ClientGLAssignments { get; set; }
        public virtual State State { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<ClientDivisionAddress> DivisionAddresses { get; set; }
    }
}