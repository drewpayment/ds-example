using Dominion.Core.Dto.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class ScoreModelEvalInfoDto
    {
        public RemarkDto Comments { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public string GroupName { get; set; }
    }
}
