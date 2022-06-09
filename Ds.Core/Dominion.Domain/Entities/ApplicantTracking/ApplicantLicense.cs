using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public class ApplicantLicense : Entity<ApplicantLicense>
    {
        public virtual int ApplicantLicenseId { get; set; }
        public virtual string Type { get; set; }
        public virtual string Description { get; set; }
        public virtual int? StateId { get; set; }
        public virtual string RegistrationNumber { get; set; }
        public virtual DateTime? ValidFrom { get; set; }
        public virtual DateTime? ValidTo { get; set; }
        public virtual int ApplicantId { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual int CountryId { get; set; }
        public virtual State State { get; set; }
        public virtual Country Country { get; set; }
    }
}
