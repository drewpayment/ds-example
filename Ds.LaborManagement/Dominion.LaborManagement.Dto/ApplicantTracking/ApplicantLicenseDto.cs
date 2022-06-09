using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public partial class ApplicantLicenseDto
    {
        public int ApplicantLicenseId { get; set; }
        public string Type { get; set; }
        public int? StateId { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public int ApplicantId { get; set; }
        public bool IsEnabled { get; set; }
        public int CountryId { get; set; }
        public string CountryDescription { get; set; }
        public string StateDescription { get; set; }
        public string Description { get; set; }

    }
}
