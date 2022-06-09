using Dominion.Core.Dto.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance.ReviewTemplates
{

    public class ReviewTemplateBaseDto
    {
        public int ReviewTemplateId { get; set; }
        public int ReviewProfileId { get; set; }
        public string Name { get; set; }
        public DateTime? ReviewProcessStartDate { get; set; }
        public DateTime? ReviewProcessEndDate { get; set; }
        public DateTime? EvaluationPeriodFromDate { get; set; }
        public DateTime? EvaluationPeriodToDate { get; set; }
        public int ClientId { get; set; }
        public bool IsArchived { get; set; }
        public bool IsRecurring { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? PayrollRequestEffectiveDate { get; set; }
        public bool IncludeReviewMeeting { get; set; }
        public ReferenceDate ReferenceDateTypeId { get; set; }
        public int? DelayAfterReference { get; set; }
        public DateUnit? DelayAfterReferenceUnitTypeId { get; set; }
        public int? ReviewProcessDuration { get; set; }
        public DateUnit? ReviewProcessDurationUnitTypeId { get; set; }
        public int? EvaluationPeriodDuration { get; set; }
        public DateUnit? EvaluationPeriodDurationUnitTypeId { get; set; }
        public IEnumerable<int> Groups { get; set; }
        public DateTime? HardCodedAnniversary { get; set; }

        public ICollection<EvaluationTemplateBaseDto> Evaluations { get; set; }
        public ICollection<ReviewTemplateReminderDto> ReviewReminders { get; set; }
        public ICollection<ReviewOwnerDto> ReviewOwners { get; set; }
    }

    public class ReviewProfileDto
    {
        public int ReviewProfileId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string DefaultInstructions { get; set; }
        public bool IncludeReviewMeeting { get; set; }
        public bool IncludeScoring { get; set; }
        public bool IncludePayrollRequests { get; set; }
        public bool IsArchived { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
    }

    public class ReviewProfileEvaluationDto : EvaluationTemplateBaseDto
    {
        public int ReviewProfileId { get; set; }
        public EvaluationRoleType EvaluationRoleTypeId { get; set; }
        public string Instructions { get; set; }
        public bool IncludeCompetencies { get; set; }
        public bool IncludeGoals { get; set; }
        public bool IncludeFeedback { get; set; }
        public bool IsGoalsWeighted { get; set; }
        public bool IsSignatureRequired { get; set; }
        public bool IsDisclaimerRequired { get; set; }
        public string DisclaimerText { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public int? OptionType { get; set; }
    }

    public class EvaluationTemplateBaseDto
    {
        public int ReviewTemplateId { get; set; }
        public int ReviewProfileEvaluationId { get; set; }
        public EvaluationRoleType Role { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public virtual int? EvaluationDuration { get; set; }
        public virtual DateUnit? EvaluationDurationUnitTypeId { get; set; }
        public int? ConductedBy { get; set; }
    }
}
