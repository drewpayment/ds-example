using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantQuestionDdlItem : Entity<ApplicantQuestionDdlItem>
    {
        public virtual int ApplicantQuestionDdlItemId { get; set; }
        public virtual int QuestionId { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsDefault { get; set; }
        public virtual string Value { get; set; }
        public virtual int? FlaggedResponse { get; set; }
        public virtual bool IsEnabled { get; set; }

        //FOREIGN KEYS
        public virtual ApplicantQuestionControl ApplicantQuestionControl { get; set; }

        public ApplicantQuestionDdlItem()
        {
        }
    }
}