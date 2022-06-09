using Dominion.Core.Dto.Misc;

namespace Dominion.Core.Dto.Client
{
    public class ClientFeaturesDto
    {
        public int ClientID { get; set; }
        public AccountFeatureEnum FeatureOptionID { get; set; }
        public string ModifiedBy { get; set; }
    }
}
