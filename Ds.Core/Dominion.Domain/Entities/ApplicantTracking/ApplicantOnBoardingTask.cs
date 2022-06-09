using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantOnBoardingTask : Entity<ApplicantOnBoardingTask>, IHasModifiedData
    {
        public virtual int ApplicantOnboardingTaskId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Description { get; set; }
        public virtual int DefaultDaysToComplete { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual int DefaultAssignedToUserId { get; set; }
        public virtual bool IsDefaultIsEmailRequired { get; set; }
        public virtual string DefaultSpecialInstructions { get; set; }
        public virtual int ProcessPhaseId { get; set; }
        public virtual LaborManagement.Dto.ApplicantTracking.ApplicantStatusType ApplicantStatusTypeId { get; set; }

        public virtual ICollection<ApplicantOnBoardingTaskAttachment> ApplicantOnBoardingTaskAttachment { get; set; }
        public virtual ICollection<ApplicantOnBoardingProcessSet> ApplicantOnBoardingProcessSet { get; set; }
        public virtual Client Client { get; set; }
        public ApplicantOnBoardingTask()
        {
        }
    }
}