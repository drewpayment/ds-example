using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantApplicationDetail : Entity<ApplicantApplicationDetail>
    {
        public virtual int ApplicationHeaderId { get; set; }
        public virtual int? QuestionId { get; set; }
        public virtual string Response { get; set; }
        public virtual bool IsFlagged { get; set; }
        public virtual int SectionId { get; set; }
        public virtual int ApplicationDetailId { get; set; }

        //FOREIGN KEYS
        public virtual ApplicantApplicationHeader ApplicantApplicationHeader { get; set; }
        public virtual ApplicationQuestionSection ApplicationQuestionSection { get; set; }
        public virtual ApplicantQuestionControl ApplicantQuestionControl { get; set; }

        public ApplicantApplicationDetail()
        {

        }
    }
}