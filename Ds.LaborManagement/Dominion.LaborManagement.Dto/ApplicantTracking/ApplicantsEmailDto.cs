using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Location;
using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Location;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public partial class ApplicantsEmailDto
    {
        public IEnumerable<ApplicantDetailDto> ApplicantsList { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}
