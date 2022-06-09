using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Core
{
    public class EmployeeImageDetail
    {
        public bool HasImage { get; set; }
        public string Url { get; set; }
    }

    public class EmployeeImageDto : EmployeeProfileImageDto
    {
        public EmployeeImageDetail ExtraLarge { get; set; }
        public EmployeeImageDetail Large { get; set; }
        public EmployeeImageDetail Medium { get; set; }
        public EmployeeImageDetail Small { get; set; }
    }
}
