using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Onboarding;
using Dominion.Domain.Entities.Forms;

namespace Dominion.Domain.Entities.Misc
{
    [Serializable]
    public class ReciprocalState : Entity<ReciprocalState>
    {
        public virtual int StateId { get; set; }
        public virtual int ReciprocalStateId { get; set; }
        public virtual State State { get; set; }
        public virtual State Reciprocal_State { get; set; }
    }
}
