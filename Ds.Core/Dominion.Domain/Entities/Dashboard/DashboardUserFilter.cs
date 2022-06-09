using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Dashboard
{
    /// <summary>
    /// EF entity for dbo.Widget table.
    /// </summary>
    public class DashboardUserFilter : Entity<DashboardUserFilter>
    {
        public virtual int FilterId               { get; set; }
        public virtual int UserId                 { get; set; }
        public virtual int DashboardWidgetId      { get; set; }
        public virtual string FilterData          { get; set; }
        public virtual DashboardWidget DashboardWidget { get; set; }

        // Entity Reference Collection
        public virtual ICollection<DashboardWidget> DashboardWidgets { get; set; }
    }
}
