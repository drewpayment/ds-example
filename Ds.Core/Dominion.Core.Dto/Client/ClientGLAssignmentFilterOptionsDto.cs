using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientGLAssignmentFilterOptionsDto
    {
        
        public bool ControlExists      { get; set; }
        public bool HasCompanyTotal    { get; set; }
        public bool SplitByCostCenter  { get; set; }
        public bool SplitByDepartment  { get; set; }
        public bool SplitByCustomClass { get; set; }
    }
}
