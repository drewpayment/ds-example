using System.Collections.Generic;

namespace Dominion.Core.Dto.Performance
{
    public class CompetencyWithModelIdsRawDto
    {
        public IEnumerable<int> CompetencyModelIds { get; set; }
        public CompetencyDto Dto { get; set; }
    }
}