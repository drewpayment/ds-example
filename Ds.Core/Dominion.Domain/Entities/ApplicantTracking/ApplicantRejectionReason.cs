using Dominion.Domain.Entities.Base;
using System;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantRejectionReason : Entity<ApplicantRejectionReason>
    {
        public virtual int ApplicantRejectionReasonId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
       

        public ApplicantRejectionReason()
        {

        }
    }
}