using System;

using Dominion.Benefits.Dto.Enrollment;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Change History entity for a <see cref="OpenEnrollment"/>.
    /// </summary>
    public class OpenEnrollmentChangeHistory : Entity<OpenEnrollmentChangeHistory>, IHasModifiedData, IOpenEnrollmentEntity, IHasChangeHistoryData
    {
        public virtual int                    OpenEnrollmentId             { get; set; }
        public virtual int                    ClientId                     { get; set; }
        public virtual OpenEnrollmentType     OpenEnrollmentTypeId         { get; set; }
        public virtual string                 Description                  { get; set; }
        public virtual DateTime               CreatedDate                  { get; set; }
        public virtual int?                   CreatedBy                    { get; set; }
        public virtual User.User              CreatedByUser                { get; set; }
        public virtual DateTime               StartDate                    { get; set; }
        public virtual DateTime               EndDate                      { get; set; }
        public virtual int                    ModifiedBy                   { get; set; }
        public virtual DateTime               Modified                     { get; set; }
        public virtual int?                   LifeEventEmployeeId          { get; set; }
        public virtual LifeEventReasonType?   LifeEventReason              { get; set; }
        public virtual DateTime?              LifeEventDate                { get; set; }
        public virtual DateTime?              DeductionsEffectiveDate      { get; set; }
        public virtual DateTime?              DeductionSentDate            { get; set; }
        public virtual int?                   DeductionSentById            { get; set; }
        public virtual bool                   IsPayrollIntegrationWaived   { get; set; }
        public virtual int                    ChangeId                     { get; set; }
        public virtual string                 ChangeMode                   { get; set; }
        public virtual LifeEventApprovalType? ApprovalStatusType           { get; set; }
        public virtual int?                   ApprovedBy                   { get; set; }
        public virtual User.User              ApprovedByUser               { get; set; }
        public virtual DateTime?              ApprovedDate                 { get; set; }
        public virtual string                 Notes                        { get; set; }
    }
}