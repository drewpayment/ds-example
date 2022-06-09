using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantResume : Entity<ApplicantResume>
    {
        public virtual int ApplicantResumeId { get; set; }
        public virtual int ApplicantId { get; set; }
        public virtual string LinkLocation { get; set; }
        public virtual DateTime DateAdded { get; set; }

        //FOREIGN KEYS
        public virtual Applicant Applicant { get; set; }

        public ApplicantResume()
        {

        }
    }
}