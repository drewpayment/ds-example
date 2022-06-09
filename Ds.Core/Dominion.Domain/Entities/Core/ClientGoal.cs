using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Core
{
    public class ClientGoal : Entity<ClientGoal>
    {
        public int ClientId { get; set; }
        public int GoalId { get; set; }

        // RELATIONSHIPS

        public virtual Client Client { get; set; }
        public virtual Goal Goal { get; set; }
    }
}
