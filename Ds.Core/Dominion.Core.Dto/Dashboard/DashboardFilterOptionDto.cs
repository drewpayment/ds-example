using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Dashboard
{
    [Serializable]
    public class DashboardFilterOptionDto
    {
        public string Type { get; set; }
        public int? Id { get; set; }
    }
}
