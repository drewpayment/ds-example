using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class CoreClientCostCenterDto
    {
        public int ClientCostCenterId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public int? DefaultGlAccountId { get; set; }
        public bool? IsDefaultGlCostCenter { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public string GlClassName { get; set; }
        public bool IsActive { get; set; }
        public bool IsExcludeFromGLFile { get; set; }

    }
}
