using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Contact.Search;
using Dominion.Core.Dto.Forms;

namespace Dominion.Core.Dto.Performance
{
    public class EvaluationDto
    {
        public int                EvaluationId              { get; set; }
        public int?               ClientId                  { get; set; }
        public int                ReviewId                  { get; set; }
        public int?               ReviewProfileId           { get; set; }
        public EvaluationRoleType Role                      { get; set; }
        public int?               EvaluatedByUserId         { get; set; }
        public int?               CurrentAssignedUserId     { get; set; }
        public DateTime?          StartDate                 { get; set; }
        public DateTime           DueDate                   { get; set; }
        public decimal?           OverallRatingValue        { get; set; }
        public DateTime?          CompletedDate             { get; set; }
        public SignatureDto       Signature                 { get; set; }
        public ContactSearchDto   EvaluatedByContact        { get; set; }
        public bool?              IsViewableByEmployee      { get; set; }
        public bool               AllowGoalWeightAssignment { get; set; }
        public int?               SignatureId               { get; set; }
        public bool?              IsApprovalProcess         { get; set; } // is on review profile but useful to have that value on an evaluation
        public virtual ICollection<ApprovalProcessHistoryDto> ApprovalProcessHistory { get; set; }
        public virtual ICollection<SignatureDto> Signatures { get; set; }
        public virtual User.UserDto CurrentAssignedUser { get; set; }
        public IEnumerable<EvaluationReminderDto> Reminders { get; set; }
        public IEnumerable<CompetencyEvaluationDto> CompetencyEvaluations { get; set; }
        public IEnumerable<FeedbackResponseData> FeedbackResponses { get; set; }
    }
}