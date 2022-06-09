using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public partial class ApplicantEmploymentHistoryDto
    {
        public int ApplicantEmploymentId { get; set; }
        public int ApplicantId { get; set; }
        public string Company { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string Zip { get; set; }
        public string Title { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool IsContactEmployer { get; set; }
        public bool? IsVoluntaryResign { get; set; }
        public string VoluntaryResignText { get; set; }
        public bool IsEnabled { get; set; }
        public string Responsibilities { get; set; }
        public int CountryId { get; set; }

        public string CompanyPlusJobTitle { get; set; }
    }
}