using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public class ApplicantReference : Entity<ApplicantReference>
    {
        public virtual int ApplicantReferenceId { get; set; }
        public virtual int ApplicantId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string Relationship { get; set; }
        public virtual int YearsKnown { get; set; }
        public virtual bool IsEnabled { get; set; }

        public ApplicantReference()
        {
        }
    }
}
