using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Dashboard
{
    /// <summary>
    /// EF entity for dbo.DashboardWidget table.
    /// </summary>
    public class DashboardWidget : Entity<DashboardWidget>
    {
        public virtual int DashboardWidgetId { get; set; }
        public virtual int DashboardId { get; set; }
        public virtual int WidgetId { get; set; }
        public virtual int Sequence { get; set; }
        public virtual DashboardConfig DashboardConfig { get; set; }
        public virtual Widget Widget { get; set; }

        // Entity Reference Collection
        public virtual ICollection<Widget> Widgets { get; set; }
        public virtual ICollection<DashboardUserFilter> DashboardUserFilters { get; set; }
    }
}
