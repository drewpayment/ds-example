using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class CoreClientDepartmentDto
    {
        public int ClientDepartmentId { get; set; }
        public int ClientDivisionId { get; set; }
        public int? DepartmentHeadEmployeeId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }

        public virtual ClientDivisionDto Division { get; set; }
    }
}
