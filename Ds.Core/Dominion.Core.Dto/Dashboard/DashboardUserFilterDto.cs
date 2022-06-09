using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Dashboard
{
    public class DashboardUserFilterDto
    {
        public virtual int FilterId          { get; set; }
        public virtual int UserId            { get; set; }
        public virtual int DashboardWidgetId { get; set; }
        public virtual string FilterData     { get; set; }
    }
}
