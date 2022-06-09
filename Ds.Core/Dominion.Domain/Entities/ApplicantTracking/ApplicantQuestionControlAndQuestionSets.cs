using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public class ApplicantQuestionControlAndQuestionSets
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
        public virtual int? SelectionCount { get; set; }

        //FOREIGN KEYS
        public virtual ApplicationQuestionSection ApplicationQuestionSection { get; set; }
        public virtual ICollection<ApplicantQuestionDdlItem> ApplicantQuestionDdlItem { get; set; }
        public virtual Client Client { get; set; }
        public int OrderId { get; set; }
    }
}
