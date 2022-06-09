using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantQuestionControl : Entity<ApplicantQuestionControl>, IHasModifiedOptionalData
    {
        public virtual int QuestionId { get; set; }
        public virtual string Question { get; set; }
        public virtual string ResponseTitle { get; set; }
        public virtual FieldType FieldTypeId { get; set; }
        public virtual int SectionId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual bool IsFlagged { get; set; }
        public virtual bool IsRequired { get; set; }
        public virtual string FlaggedResponse { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual int? SelectionCount { get; set; }

        //FOREIGN KEYS
        public virtual ApplicationQuestionSection ApplicationQuestionSection { get; set; }
        public virtual ICollection<ApplicantQuestionDdlItem> ApplicantQuestionDdlItem { get; set; }
        public virtual ICollection<ApplicantQuestionSet> ApplicantQuestionSets { get; set; }
        public virtual Client Client { get; set; }

        public ApplicantQuestionControl()
        {
        }
    }
}