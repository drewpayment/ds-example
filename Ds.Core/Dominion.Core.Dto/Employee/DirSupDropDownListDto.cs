using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Dominion.Core.Dto.Employee
{
    public class DirSupDropDownListDto
    {
        public int? finalSupervisor { get; set; }
        public int? originalSupervisor { get; set; }
    }
}
