using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.EEOC;

namespace Dominion.Core.Dto.Client
{
    public class ClientDivisionDto
    {
        public int ClientDivisionId { get; set; }
        public int ClientId { get; set; }
        public virtual int? ClientContactId { get; set; }
        public string Name { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string City { get; set; }
        public virtual string StateAbbreviation { get; set; }
        public virtual int StateId { get; set; }
        public virtual string Zip { get; set; }
        public virtual int CountryId { get; set; }
        public virtual byte SendCorrespondenceTo { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual bool? IsUseSeparateAccountRoutingNumber { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual int? BankId { get; set; }
        public bool IsActive { get; set; }
        public virtual bool IsUseAsStubAddress { get; set; }
        public bool HasClientGLAssignment { get; set; }
        public virtual string LocationId { get; set; }
        public virtual DateTime? DateLocationOpened { get; set; }
        public virtual DateTime? DateLocationClosed { get; set; }

        public ClientDto Client { get; set; }
        //public virtual ICollection<EEO1ExportDto.EmployeeDto> Employees { get; set; }
        public virtual ICollection<CoreClientDepartmentDto> Departments { get; set; }
        public IEnumerable<ClientGLAssignmentDto> ClientGLAssignments { get; set; }
    }

    public class ClientDivisionBasicDto
    {
        public int ClientDivisionId { get; set; }
        public int ClientId { get; set; }
        public virtual int? ClientContactId { get; set; }
        public string Name { get; set; }
    }
}
