using System;

using Dominion.Benefits.Dto.Enrollment;

namespace Dominion.Domain.Entities.Benefit
{
    public interface IOpenEnrollmentEntity
    {
        int                  OpenEnrollmentId             { get; set; }
        int                  ClientId                     { get; set; }
        OpenEnrollmentType   OpenEnrollmentTypeId         { get; set; }
        string               Description                  { get; set; }
        DateTime             CreatedDate                  { get; set; }
        DateTime             StartDate                    { get; set; }
        DateTime             EndDate                      { get; set; }
        int                  ModifiedBy                   { get; set; }
        DateTime             Modified                     { get; set; }
        int?                 LifeEventEmployeeId          { get; set; }
        LifeEventReasonType? LifeEventReason              { get; set; }
        DateTime?            LifeEventDate                { get; set; }
        DateTime?            DeductionsEffectiveDate      { get; set; }
        DateTime?            DeductionSentDate            { get; set; }
        int?                 DeductionSentById            { get; set; }
        bool                 IsPayrollIntegrationWaived   { get; set; }
    }
}