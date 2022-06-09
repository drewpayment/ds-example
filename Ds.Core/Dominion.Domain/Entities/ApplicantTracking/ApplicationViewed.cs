using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicationViewed : Entity<ApplicationViewed>
    {
        public virtual int ApplicationViewedId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int UserId { get; set; }
        public virtual int ApplicationHeaderId { get; set; }
        public virtual DateTime ViewedOn { get; set; }

        //FOREIGN KEYS
        public virtual Client Client { get; set; }
        public virtual Dominion.Domain.Entities.User.User User { get; set; }
        public virtual ApplicantApplicationHeader ApplicantApplicationHeader { get; set; }

        public ApplicationViewed()
        {

        }
    }
}