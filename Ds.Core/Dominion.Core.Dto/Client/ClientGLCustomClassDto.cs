using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientGLCustomClassDto
    {
        public int ClientGLCustomClassId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public int ClientDivisionId { get; set; }
        public int ClientGroupId { get; set; }
        public int ClientDepartmentId { get; set; }
        public int ClientCostCenterId { get; set; }
    }
}
