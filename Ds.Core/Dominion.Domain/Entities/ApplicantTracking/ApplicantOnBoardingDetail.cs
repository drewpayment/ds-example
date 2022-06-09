using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantOnBoardingDetail : Entity<ApplicantOnBoardingDetail>
    {
        public virtual int ApplicantOnBoardingDetailId { get; set; }
        public virtual int ApplicantOnBoardingHeaderId { get; set; }
        public virtual int ApplicantOnBoardingTaskId { get; set; }
        public virtual int AssignedToUserId { get; set; }
        public virtual bool IsEmailRequired { get; set; }
        public virtual int DaysToComplete { get; set; }
        public virtual string SpecialInstructions { get; set; }
        public virtual DateTime? DateStarted { get; set; }
        public virtual DateTime? DateCompleted { get; set; }
        public virtual bool IsCompleted { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual int ApplicantCorrespondenceId { get; set; }
        public virtual DateTime? Modified { get; set; }

        public ApplicantOnBoardingDetail()
        {
        }
    }
}