using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public partial class Feedback : Entity<Feedback>, IHasModifiedData
    {
        public int       FeedbackId          { get; set; } 
        public int?      ClientId            { get; set; } 
        public FieldType FieldTypeId         { get; set; } 
        public string    Body                { get; set; } 
        public bool      IsSupervisor        { get; set; } 
        public bool      IsPeer              { get; set; } 
        public bool      IsSelf              { get; set; } 
        public bool      IsActionPlan        { get; set; } 
        public bool      IsRequired          { get; set; } 
        public DateTime  Modified            { get; set; } 
        public int       ModifiedBy          { get; set; } 
        public bool      IsVisibleToEmployee { get; set; }
        public bool      IsArchived          { get; set; }


        public virtual Client Client { get; set; }
        public virtual FieldTypeInfo FieldTypeInfo { get; set; }
        public virtual ICollection<FeedbackItem> FeedbackItems { get; set; } 
        public virtual ICollection<FeedbackResponse> FeedbackResponses { get; set; }
        public virtual ICollection<ReviewProfileEvaluationFeedback> ReviewProfileEvaluationFeedbacks { get; set; }
    }
}
