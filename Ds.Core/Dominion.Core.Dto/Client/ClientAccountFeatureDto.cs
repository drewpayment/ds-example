using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Misc;

namespace Dominion.Core.Dto.Client
{
    public class ClientAccountFeatureDto
    {
        // Basic Properties
        public int ClientId { get; set; }
        public AccountFeatureEnum AccountFeature { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime Modifed { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsExisting { get; set; }
        public bool IsClientEnabled { get; set; }
        public string Description { get; set; }
        public int FeatureOptionsGroupId { get; set; }
        public int AutomaticBillingItemCount { get; set; }
    }
}
