using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.App
{
    public class ApplicationMenu : Entity<ApplicationMenu>, IHasModifiedData
    {
        public int      MenuId     { get; set; }
        public string   Name       { get; set; }
        public DateTime Modified   { get; set; }
        public int      ModifiedBy { get; set; }

        public virtual ICollection<ApplicationMenuItem> MenuItems { get; set; }
    }
}
