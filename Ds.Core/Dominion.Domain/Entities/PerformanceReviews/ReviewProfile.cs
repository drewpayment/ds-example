using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    /// <summary>
    /// This defines what <see cref="Evaluation"/>s make up a <see cref="Review"/>, what items are included in those
    /// <see cref="Evaluation"/>s, and which features should be used in the <see cref="Review"/>.
    /// </summary>
    public partial class ReviewProfile : Entity<ReviewProfile>, IHasModifiedData
    {
        public int      ReviewProfileId        { get; set; } 
        public int      ClientId               { get; set; }
        public string   Name                   { get; set; } 
        public string   DefaultInstructions    { get; set; } 
        public bool     IncludeReviewMeeting   { get; set; }
        public bool     IncludeScoring         { get; set; }
        public bool     IncludePayrollRequests { get; set; }
        public bool     IsArchived             { get; set; }
        public DateTime Modified               { get; set; } 
        public int      ModifiedBy             { get; set; } 

        //REVERSE NAVIGATION
        public virtual ICollection<ReviewProfileEvaluation> ReviewProfileEvaluations { get; set; } // many-to-one;
        public virtual Client Client { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<ReviewTemplate> ReviewTemplates { get; set; }
    }
}
