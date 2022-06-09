using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    [Serializable]
    public partial class ApplicantsAutoDenialDto
    {
        public int ApplicantId { get; set; }
        public string Email { get; set; }
        public int ApplicationHeaderId { get; set; }
        public string ClientName { get; set; }
    }
}