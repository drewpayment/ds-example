using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantPostingOwner : Entity<ApplicantPostingOwner>
    {
        public virtual int PostingId { get; set; }
        public virtual int UserId { get; set; }

        //FOREIGN KEYS
        public virtual ApplicantPosting ApplicantPosting { get; set; }
        public virtual Dominion.Domain.Entities.User.User User { get; set; }

        public ApplicantPostingOwner()
        {

        }
    }
}