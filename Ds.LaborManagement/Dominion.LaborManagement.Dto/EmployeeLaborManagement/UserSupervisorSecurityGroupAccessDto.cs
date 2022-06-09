using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Clients;

namespace Dominion.LaborManagement.Dto.EmployeeLaborManagement
{
    public class UserSupervisorSecurityGroupAccessDto
    {
        public int UserId { get; set; }
        public int ClientId { get; set; }
        public int? ClientCostCenterId { get; set; }
        public ClientCostCenter ClientCostCenter { get; set; }
        
    }
}
