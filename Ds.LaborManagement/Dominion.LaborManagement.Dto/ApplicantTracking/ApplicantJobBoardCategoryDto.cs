using System;
using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    [Serializable]
    public partial class ApplicantJobBoardCategoryDto
    {
        public int PostingCategoryId { get; set; }
        public string Name { get; set; }
        public int ClientId { get; set; }
        public bool IsEnabled { get; set; }
        public string Description { get; set; }
        public int PostingCount { get; set; }
        public string CompanyName { get; set; }

        public IEnumerable<ApplicantJobBoardPostingDto> Postings { get; set; }
    }
}
