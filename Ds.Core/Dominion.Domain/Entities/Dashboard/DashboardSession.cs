using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Dashboard
{
    /// <summary>
    /// EF entity for dbo.Widget table.
    /// </summary>
    public class DashboardSession : Entity<DashboardSession>
    {
        public virtual int UserId     { get; set; }
        public virtual int DashboardId { get; set; }
        public virtual string FilterData         { get; set; }
        public virtual DashboardConfig DashboardConfig { get; set; }
    }
}
