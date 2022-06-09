using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    [Serializable]
    public partial class ApplicantStatusTypeDto
    {
        public ApplicantStatusType ApplicantStatusId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
    }
}