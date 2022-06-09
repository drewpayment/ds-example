using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class CompetencyOptionsDto
    {
        public int OptionId { get; set; }
        public bool IsDisabled { get; set; }
        public bool EnforceRequiredComments { get; set; }
        public ICollection<CompetencyRateCommentRequiredDto> CompetencyRateCommentRequired { get; set; }
    }
}
