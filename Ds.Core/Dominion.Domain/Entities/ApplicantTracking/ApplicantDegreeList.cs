using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantDegreeList : Entity<ApplicantDegreeList>
    {
        public virtual int DegreeId { get; set; }
        public virtual string Description { get; set; }

        public ApplicantDegreeList()
        {

        }
    }
}