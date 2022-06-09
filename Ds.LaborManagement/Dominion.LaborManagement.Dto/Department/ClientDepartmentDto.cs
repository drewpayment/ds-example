using Dominion.Core.Dto.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Department
{
    public class ClientDepartmentDto
    {
        public int ClientDepartmentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ClientId { get; set; }
        public DateTime? Modified { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public int? DepartmentHeadEmployeeId { get; set; }
        public int ClientDivisionId { get; set; }
        public bool HasClientGLAssignment { get; set; }
        public bool ModifyAllDivisions { get; set; }
        public IEnumerable<ClientGLAssignmentDto> ClientGLAssignments { get; set; }
    }
}
