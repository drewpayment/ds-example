using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.Utility.Mapping;

namespace Dominion.LaborManagement.Service.Mapping
{
    public class ApplicantEducationHistoryMapper
    {
        public class
            ToApplicantEducationHistoryDto : ExpressionMapper<ApplicantEducationHistory, ApplicantEducationHistoryDto>
        {
            public override Expression<Func<ApplicantEducationHistory, ApplicantEducationHistoryDto>> MapExpression
            {
                get
                {
                    return education => new ApplicantEducationHistoryDto()
                    {
                        IsEnabled = education.IsEnabled,
                        ApplicantEducationId = education.ApplicantEducationId,
                        ApplicantId = education.ApplicantId,
                        ApplicantSchoolTypeId = education.ApplicantSchoolTypeId,
                        ApplicantSchoolType = education.ApplicantSchoolType.Description,
                        DateEnded = education.DateEnded,
                        DateStarted = education.DateStarted,
                        DegreeId = education.DegreeId,
                        Description = education.Description
                    };
                }
            }
        }
    }
}
