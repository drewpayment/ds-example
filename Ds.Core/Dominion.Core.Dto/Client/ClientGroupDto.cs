using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientGroupDto
    {
        public int ClientGroupId { get; set; }
        public int ClientId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool HasClientGLAssignment { get; set; }
        public IEnumerable<ClientGLAssignmentDto> ClientGLAssignments { get; set; }

    }
}
