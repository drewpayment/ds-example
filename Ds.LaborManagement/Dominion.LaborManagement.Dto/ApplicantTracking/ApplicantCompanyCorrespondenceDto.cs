using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    [Serializable]
    public partial class ApplicantCompanyCorrespondenceDto
    {
        public int ApplicantCompanyCorrespondenceId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public Dto.ApplicantTracking.ApplicantCorrespondenceType? ApplicantCorrespondenceTypeId { get; set; }
        public string ApplicantCorrespondenceType { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsActive { get; set; }
        public bool IsText { get; set; }
        public bool? IsApplicantAdmin { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }
    }
}