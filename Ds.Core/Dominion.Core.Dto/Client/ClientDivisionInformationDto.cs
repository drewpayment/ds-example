using Dominion.Core.Dto.Banks;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Location;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Client
{
    public class ClientDivisionInformationDto
    {
        public List<ClientDivisionDto> Divisions { get; set; }
        public List<StateDto> States { get; set; }
        public List<CountryDto> Countries { get; set; }
        public List<ClientContactDto> Contacts { get; set; }
        public List<EmployeeFullDto> Employees { get; set; }
        public List<ClientDivisionAddressDto> Addresses { get; set; }
        public List<BankBasicDto> Banks { get; set; }
        public List<ClientDivisionLogoDto> Logos { get; set; }
        public bool isWotc { get; set; }
        public List<int?> BlockedDivisions { get; set; }
        public List<CoreClientDepartmentDto> Departments { get; set; }
        public List<ClientGLMappingItemDto> GLAssignments { get; set; }
        public int MaxLogoFileSizeBytes { get; set; }
    }
}
