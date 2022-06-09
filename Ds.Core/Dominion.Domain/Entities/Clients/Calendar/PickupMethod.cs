using System.Collections.Generic;

using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Clients.Calendar
{
    public partial class PickupMethod : Entity<PickupMethod>
    {
        public virtual byte   PickupMethodId   { get; set; } 
        public virtual string Name               { get; set; } 
        public virtual string Code               { get; set; } 
    }
}
