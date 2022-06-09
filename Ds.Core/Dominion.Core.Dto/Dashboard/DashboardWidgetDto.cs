using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Dashboard
{
    public class DashboardWidgetDto
    {
        public int DashboardWidgetId { get; set; }
        public int DashboardId       { get; set; }
        public int WidgetId          { get; set; }
        public int Sequence          { get; set; }
    }
}
