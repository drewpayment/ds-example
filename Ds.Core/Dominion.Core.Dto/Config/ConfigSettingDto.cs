using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Future Consideration: Consolidate with Person & Address
namespace Dominion.Core.Dto.Configs
{
    public partial class ConfigSettingDto
    {
        public int ConfigSettingId { get; set; }
        public int    ConfigId { get; set; }
        public string ConfigSettingKey { get; set; }
        public string ConfigSettingValue { get; set; }
    }
}
