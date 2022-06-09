using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Dashboard
{
    /// <summary>
    /// EF entity for dbo.DashboardConfig table.
    /// </summary>
    public class DashboardConfig : Entity<DashboardConfig>
    {
        public virtual int DashboardId  { get; set; }
        public virtual string Name      { get; set; }

        // Entity Reference Collection
        public virtual ICollection<DashboardWidget> DashboardWidgets { get; set; }
    }
}
