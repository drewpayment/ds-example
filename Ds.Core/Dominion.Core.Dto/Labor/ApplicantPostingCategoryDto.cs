using System;
using System.Collections.Generic;


namespace Dominion.Core.Dto.Labor
{
    [Serializable]
    public partial class ApplicantPostingCategoryDto
    {
        public int PostingCategoryId { get; set; }
        public string Name { get; set; }
        public int ClientId { get; set; }
        public bool IsEnabled { get; set; }
        public string Description { get; set; }
        public int PostingCount { get; set; }
        public string CompanyName { get; set; }

        public IEnumerable<ApplicantPostingDetailDto> Postings { get; set; }
    }
}
