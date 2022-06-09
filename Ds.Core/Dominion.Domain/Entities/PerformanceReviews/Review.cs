using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public partial class Review : Entity<Review>, IHasModifiedData
    {
        public int       ReviewId                 { get; set; } 
        public int       ReviewedEmployeeId       { get; set; } 
        public int?      ReviewProfileId          { get; set; }
        public int?      ReviewTemplateId      { get; set; }
        public string    Title                    { get; set; } 
        public int?      ReviewOwnerUserId        { get; set; }
        /// <summary>
        /// The start of the duration that will be assessed for the employee's performance
        /// </summary>
        public DateTime? EvaluationPeriodFromDate { get; set; }
        /// <summary>
        /// The end of the duration that will be assessed for the employee's performance
        /// </summary>
        public DateTime? EvaluationPeriodToDate   { get; set; }
        /// <summary>
        ///
        /// </summary>
        public DateTime? ReviewProcessStartDate   { get; set; } 
        public DateTime? ReviewProcessDueDate     { get; set; } 
        public decimal?  OverallRatingValue       { get; set; } 
        /// <summary>
        /// Text to display at the top of each evaluation.
        /// </summary>
        public string    Instructions             { get; set; } 
        public DateTime? ReviewCompletedDate      { get; set; } 
        /// <summary>
        /// NOT USED
        /// </summary>
        public string    JsonData                 { get; set; } 
        public bool      IsReviewMeetingRequired  { get; set; }
        public bool      IsGoalsWeighted          { get; set; }
        public DateTime  Modified                 { get; set; } 
        public int       ModifiedBy               { get; set; }

        /// <summary>
        /// If accessing this property in a linq to entities query use the <see cref="ReviewMaps.FromReview.ToOverallScore"/> expression to 
        /// grab this property.
        /// 
        /// OverallScore should only be set if the client has not set up Scoreing and EvaluationGroups
        /// </summary>
        public decimal?   OverallScore             { get; set; }
        /// <summary>
        /// CompetencyScore should only be set if the client has not set up Scoreing and EvaluationGroups
        /// </summary>
        public decimal?   CompetencyScore          { get; set; }
        /// <summary>
        /// GoalScore should only be set if the client has not set up Scoreing and EvaluationGroups
        /// </summary>
        public decimal?   GoalScore                { get; set; }

        //REVERSE NAVIGATION
        public virtual ICollection<Evaluation> Evaluations { get; set; }
        public virtual ICollection<ReviewMeeting> ReviewMeetings { get; set; }
        public virtual Employee.Employee ReviewedEmployee { get; set; }
        public virtual User.User ReviewOwnerUser { get; set; }
        public virtual ReviewProfile ReviewProfile { get; set; }
        public virtual ReviewTemplate ReviewTemplate { get; set; }
        public virtual ICollection<ReviewProposal> ReviewProposals { get; set; }
        public virtual ICollection<ReviewRemark> Remarks { get; set; }
    }
}