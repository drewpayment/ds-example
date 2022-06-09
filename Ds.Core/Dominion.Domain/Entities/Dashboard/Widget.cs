using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Dashboard
{
    /// <summary>
    /// EF entity for dbo.Widget table.
    /// </summary>
    public class Widget : Entity<Widget>
    {
        public virtual int WidgetId     { get; set; }
        public virtual string Name      { get; set; }
        public virtual int Size         { get; set; }

        // Entity Reference Collection
        public virtual ICollection<DashboardWidget> DashboardWidgets { get; set; }
    }
}
