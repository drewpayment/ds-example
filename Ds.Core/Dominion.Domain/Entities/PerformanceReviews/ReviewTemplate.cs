using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Misc;
using Dominion.Core.Dto.Performance;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    /// <summary>
    /// Ties a <see cref="Review"/> to a <see cref="PerformanceReviews.ReviewProfile"/>.  This also determines the
    /// default values for the various date ranges of the <see cref="Review"/>.  Everything needed
    /// to create a <see cref="Review"/> can be accessed from this record.
    /// </summary>
    public partial class ReviewTemplate : Entity<ReviewTemplate>, IHasModifiedData
    {
        public virtual int       ReviewTemplateId         { get; set; } 
        public virtual int       ReviewProfileId             { get; set; } 
        public virtual string    Name                        { get; set; } 
        public virtual DateTime? ReviewProcessStartDate      { get; set; } 
        public virtual DateTime? ReviewProcessEndDate        { get; set; } 
        public virtual DateTime? EvaluationPeriodFromDate    { get; set; } 
        public virtual DateTime? EvaluationPeriodToDate      { get; set; } 
        public virtual DateTime  Modified                    { get; set; } 
        public virtual int       ModifiedBy                  { get; set; } 
        public virtual DateTime? PayrollRequestEffectiveDate { get; set; }
        public virtual int ClientId { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual bool IsRecurring { get; set; }
        public ReferenceDate ReferenceDateTypeId { get; set; }
        public int? DelayAfterReference { get; set; }
        public DateUnit? DelayAfterReferenceUnitTypeId { get; set; }
        public int? ReviewProcessDuration { get; set; }
        public DateUnit? ReviewProcessDurationUnitTypeId { get; set; }
        public int? EvaluationPeriodDuration { get; set; }
        public DateUnit? EvaluationPeriodDurationUnitTypeId { get; set; }
        public DateTime? HardCodedAnniversary { get; set; }

        //REVERSE NAVIGATION
        public virtual ICollection<EvaluationTemplate> EvaluationTemplates { get; set; } // many-to-many
        public virtual ICollection<Review> EmployeeReviews { get; set; }
        public virtual ICollection<ReviewTemplateGroup>  ReviewTemplateGroups { get; set; }
        public virtual ICollection<ReviewTemplateReminder> ReviewTemplateReminders { get; set; }
        public virtual ICollection<ReviewOwner> ReviewOwners { get; set; }


        //FOREIGN KEYS
        public virtual ReviewProfile ReviewProfile { get; set; }
        public virtual Client Client { get; set; }
    }
}