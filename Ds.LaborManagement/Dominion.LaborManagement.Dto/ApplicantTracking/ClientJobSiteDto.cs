using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.LaborManagement.Dto.Enums;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ClientJobSiteDto
    {
        public int ClientJobSiteId { get; set; }
        public int ClientId { get; set; }
        public ApplicantJobSiteEnum ApplicantJobSiteId { get; set; }
        public string Code { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public int Counter { get; set; }
        public string JobSiteDescription { get; set; }
        public bool? SharePosts { get; set; }
        public string Email { get; set; }
    }
}
