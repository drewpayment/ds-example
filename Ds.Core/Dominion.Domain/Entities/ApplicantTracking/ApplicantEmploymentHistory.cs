using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;
using Dominion.Domain.Entities.Misc;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantEmploymentHistory : Entity<ApplicantEmploymentHistory>
    {
        public virtual int ApplicantEmploymentId { get; set; }
        public virtual int ApplicantId { get; set; }
        public virtual string Company { get; set; }
        public virtual string City { get; set; }
        public virtual int StateId { get; set; }
        public virtual string Zip { get; set; }
        public virtual string Title { get; set; }
        public virtual string StartDate { get; set; }
        public virtual string EndDate { get; set; }
        public virtual bool IsContactEmployer { get; set; }
        public virtual bool? IsVoluntaryResign { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual string Responsibilities { get; set; }
        public virtual int CountryId { get; set; }

        //FOREIGN KEYS
        public virtual Applicant Applicant { get; set; }
    }
}