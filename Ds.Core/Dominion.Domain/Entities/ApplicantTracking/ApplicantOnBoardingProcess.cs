using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantOnBoardingProcess : Entity<ApplicantOnBoardingProcess>, IHasModifiedData
    {
        public virtual int ApplicantOnboardingProcessId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Description { get; set; }
        public virtual int? CustomToPostingId { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual int ApplicantOnBoardingProcessTypeId { get; set; }
        public virtual int ProcessPhaseId { get; set; }
        public virtual ICollection<ApplicantPosting> ApplicantPostings { get; set; }
        public virtual ICollection<ApplicantOnBoardingProcessSet> ApplicantOnBoardingProcessSets { get; set; }
        public virtual Client Client { get; set; }
        public virtual ApplicantOnBoardingProcessType ApplicantOnBoardingProcessType { get; set; }
        public ApplicantOnBoardingProcess()
        {
        }
    }
}