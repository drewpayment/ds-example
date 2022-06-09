using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantOnBoardingDefaultDetail : Entity<ApplicantOnBoardingDefaultDetail>
    {
        public virtual int ApplicantOnBoardingDefaultDetailId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int ApplicantPostingId { get; set; }
        public virtual int ApplicantOnBoardingProcessId { get; set; }
        public virtual int ApplicantOnBoardingTaskId { get; set; }
        public virtual int AssignedToUserId { get; set; }
        public virtual bool IsEmailRequired { get; set; }
        public virtual int DaysToComplete { get; set; }
        public virtual string SpecialInstructions { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual bool IsAutoStart { get; set; }

        public ApplicantOnBoardingDefaultDetail()
        {
        }
    }
}