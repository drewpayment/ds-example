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
    public class ApplicantLicenseMapper
    {
        public class ToApplicantLicense : ExpressionMapper<ApplicantLicenseDto, ApplicantLicense>
        {
            public override Expression<Func<ApplicantLicenseDto, ApplicantLicense>> MapExpression
            {
                get
                {
                    return licenseDto => new ApplicantLicense()
                    {
                        ApplicantLicenseId = licenseDto.ApplicantLicenseId,
                        ApplicantId = licenseDto.ApplicantId,
                        CountryId = licenseDto.CountryId,
                        IsEnabled = licenseDto.IsEnabled,
                        RegistrationNumber = licenseDto.RegistrationNumber,
                        StateId = licenseDto.StateId,
                        Type = licenseDto.Type,
                        ValidFrom = licenseDto.ValidFrom,
                        ValidTo = licenseDto.ValidTo,
                        Description = licenseDto.Description
                    };
                }
            }
        }

        public class ToApplicantLicenseDto : ExpressionMapper<ApplicantLicense, ApplicantLicenseDto>
        {
            public override Expression<Func<ApplicantLicense, ApplicantLicenseDto>> MapExpression
            {
                get
                {
                    return license => new ApplicantLicenseDto()
                    {
                        ApplicantLicenseId = license.ApplicantLicenseId,
                        ApplicantId = license.ApplicantId,
                        CountryId = license.CountryId,
                        IsEnabled = license.IsEnabled,
                        RegistrationNumber = license.RegistrationNumber,
                        StateId = license.StateId,
                        Type = license.Type,
                        ValidFrom = license.ValidFrom,
                        ValidTo = license.ValidTo,
                        CountryDescription = license.Country.Name,
                        StateDescription = license.State != null ? license.State.Abbreviation : "",
                        Description = license.Description
                };
                }
            }
        }
    }
}
