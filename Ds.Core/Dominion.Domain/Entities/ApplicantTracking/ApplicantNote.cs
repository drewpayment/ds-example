using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantNote : Entity<ApplicantNote>
    {
        public virtual int ApplicantId { get; set; }
        public virtual int RemarkID { get; set; }
        
        //FOREIGN KEYS
        public virtual Dominion.Domain.Entities.Core.Remark Remark { get; set; }
        public virtual Applicant Applicant { get; set; }

        public ApplicantNote()
        {

        }
    }
}