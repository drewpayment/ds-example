using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Dashboard
{
    public class DashboardObjectDto
    {
        public virtual int DashboardWidgetId { get; set; }
        public virtual int DashboardId { get; set; }
        public virtual int Size { get; set; }
        public virtual int Sequence { get; set; }
        public virtual int WidgetId { get; set; }
        public virtual string WidgetName { get; set; }
    }
}
