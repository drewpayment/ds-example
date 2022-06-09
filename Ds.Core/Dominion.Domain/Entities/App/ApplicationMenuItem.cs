using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.App
{
    public class ApplicationMenuItem : Entity<ApplicationMenuItem>, IHasModifiedData
    {
        public int      MenuItemId  { get; set; }
        public int      MenuId      { get; set; }
        public int?     ParentId    { get; set; }
        public short    ParentIndex { get; set; }
        public string   Title       { get; set; }
        public int?     ResourceId  { get; set; }
        public DateTime Modified    { get; set; }
        public int      ModifiedBy  { get; set; }
        public virtual ApplicationResource Resource { get; set; }
        public virtual ApplicationMenu Menu { get; set; }
        public virtual ApplicationMenuItem ParentMenuItem { get; set; }
        public virtual ICollection<ApplicationMenuItem> ChildMenuItems { get; set; }
    }
}