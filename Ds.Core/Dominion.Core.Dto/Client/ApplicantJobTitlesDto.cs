using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ApplicantJobTitlesDto
    {
        public int JobTitleId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Requirements { get; set; }
        public bool IsActive { get; set; }
    }
}
