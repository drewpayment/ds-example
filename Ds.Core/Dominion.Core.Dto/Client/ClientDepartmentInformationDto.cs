using Dominion.Core.Dto.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientDepartmentInformationDto
    {
        public Boolean HasDepartmentsAcrossDivisionsOption { get; set; }
        public List<ClientDivisionDto> Divisions { get; set; }
        public List<CoreClientDepartmentDto> Departments { get; set; }
        public IEnumerable<EmployeeBasicDto> HeadOfDepartments { get; set; }
        public List<int?> BlockedDepartments { get; set; }
    }
}
