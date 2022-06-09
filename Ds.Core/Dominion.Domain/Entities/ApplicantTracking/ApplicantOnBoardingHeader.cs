using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantOnBoardingHeader : Entity<ApplicantOnBoardingHeader>
    {
        public virtual int ApplicantOnBoardingHeaderId { get; set; }
        public virtual int ApplicantApplicationHeaderId { get; set; }
        public virtual int ApplicantOnBoardingProcessId { get; set; }
        public virtual int ApplicantId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual DateTime? OnBoardingStarted { get; set; }
        public virtual DateTime? OnBoardingEnded { get; set; }
        public virtual bool IsHired { get; set; }
        public virtual bool IsRejected { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual DateTime Modified { get; set; }

        //FOREIGN KEYS
        public virtual ApplicantApplicationHeader ApplicantApplicationHeader { get; set; }
        public virtual ApplicantOnBoardingProcess ApplicantOnBoardingProcess { get; set; }
        public virtual Applicant Applicant { get; set; }
        public virtual Client Client { get; set; }

        public ApplicantOnBoardingHeader()
        {
        }
    }
}