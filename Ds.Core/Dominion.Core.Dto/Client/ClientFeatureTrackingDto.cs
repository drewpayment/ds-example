using System;
using Dominion.Core.Dto.Misc;

namespace Dominion.Core.Dto.Client
{
    public class ClientFeatureTrackingDto
    {
        public int                ClientFeatureTrackingId { get; set; }
        public int                ClientId                { get; set; }
        public AccountFeatureEnum FeatureOptionId         { get; set; }
        public bool               IsEnabled               { get; set; }
        public DateTime           Modified                { get; set; }
        public int                ModifiedBy              { get; set; }
    }
}
