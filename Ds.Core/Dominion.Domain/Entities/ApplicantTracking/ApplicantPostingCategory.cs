using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantPostingCategory : Entity<ApplicantPostingCategory>, IHasModifiedOptionalData
    {
        public virtual int PostingCategoryId { get; set; }
        public virtual string Name { get; set; }
        public virtual int ClientId { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual string Description { get; set; }
        public virtual Client Client { get; set; }
        public virtual ICollection<ApplicantPosting> ApplicantPostings { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual DateTime? Modified { get; set; }

        public ApplicantPostingCategory()
        {
        }
    }
}