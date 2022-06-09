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
    public class AppHistoryMaps
    {
        public class ToApplicantEmploymentHistoryDto : ExpressionMapper<ApplicantEmploymentHistory, ApplicantEmploymentHistoryDto>
        {
            public ToApplicantEmploymentHistoryDto()
            {

            }

            public override Expression<Func<ApplicantEmploymentHistory, ApplicantEmploymentHistoryDto>> MapExpression =>
                c => new ApplicantEmploymentHistoryDto()
                {
                    IsEnabled = c.IsEnabled,
                    ApplicantEmploymentId = c.ApplicantEmploymentId,
                    ApplicantId = c.ApplicantId,
                    City = c.City,
                    Company = c.Company,
                    CountryId = c.CountryId,
                    EndDate = c.EndDate,
                    IsContactEmployer = c.IsContactEmployer,
                    IsVoluntaryResign = c.IsVoluntaryResign
                };
        }
    }
}
