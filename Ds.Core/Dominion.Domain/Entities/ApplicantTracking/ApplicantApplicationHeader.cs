using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using System;
using Dominion.Domain.Entities.Core;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantApplicationHeader : Entity<ApplicantApplicationHeader>
    {
        public virtual int ApplicationHeaderId { get; set; }
        public virtual int ApplicantId { get; set; }
        public virtual int PostingId { get; set; }
        public virtual bool IsApplicationCompleted { get; set; }
        public virtual bool IsRecommendInterview { get; set; }        
        public virtual DateTime? DateSubmitted { get; set; }
        public virtual int ApplicantResumeId { get; set; }
        public virtual int? ApplicantRejectionReasonId { get; set; }
        public virtual int OrigPostingId { get; set; }
        public virtual LaborManagement.Dto.ApplicantTracking.ApplicantStatusType? ApplicantStatusTypeId { get; set; }
        public virtual string CoverLetter { get; set; }
        public virtual int? CoverLetterId { get; set; }
        public virtual bool AddedByAdmin { get; set; }
        public virtual int? AddedBy { get; set; }
        public virtual int? Score { get; set; }
        public virtual int? DisclaimerId { get; set; }
        //FOREIGN KEYS
        /// <summary>
        /// The id of the record in some third party system.  For example, when an application came
        /// from Indeed, this will store Indeed's id of the application.  When the application
        /// is from our own system, this is null.
        /// </summary>
        public virtual ExternalApplicationIdentity ExternalApplicationIdentity { get; set; }
        public virtual ApplicantPosting ApplicantPosting { get; set; }
        public virtual Applicant Applicant { get; set; }
        public virtual ApplicantResume ApplicantResume { get; set; }
        public virtual ICollection<ApplicantDocument> ApplicantDocument { get; set; }
        public virtual ApplicantRejectionReason ApplicantRejectionReason { get; set; }
        public virtual ApplicantStatusTypeDetail ApplicantStatusType { get; set; }        
        public virtual ICollection<ApplicantApplicationEmailHistory> ApplicantApplicationEmailHistory { get; set; }
        public virtual Resource CoverLetterResource { get; set; }
        public virtual ICollection<ApplicationViewed> ApplicationViewed { get; set; }
        public virtual ICollection<ApplicantApplicationDetail> ApplicantApplicationDetail { get; set; }
        public virtual ICollection<ApplicantOnBoardingHeader> ApplicantOnBoardingHeader { get; set; }
        public virtual User.User AddedByUser { get; set; }

        public ApplicantApplicationHeader()
        {
        }
    }
}