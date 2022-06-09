using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class ScoreModelSettingsDto
    {
        public bool IsScoringEnabled { get; set; }
        public bool IsPreferencePercent { get; set; }
        public bool IsPayrollRequestsEnabled { get; set; }
        public bool IsScoreEmployeeViewable { get; set; }
    }
}
