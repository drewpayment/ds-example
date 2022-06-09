using Dominion.Core.Dto.Client;
using System;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Labor
{
    public class ClientCostCenterDto
    {
        public virtual int ClientCostCenterId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
        public virtual bool? IsDefaultGlCostCenter { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual bool IsActive { get; set; }
        public IEnumerable<ClientGLAssignmentDto> ClientGLAssignments { get; set; }
    }
}
