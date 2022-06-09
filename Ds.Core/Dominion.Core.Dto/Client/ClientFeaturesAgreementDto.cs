
using Dominion.Core.Dto.Misc;

namespace Dominion.Core.Dto.Client
{
    public class ClientFeaturesAgreementDto
    {
        public int ClientFeaturesAgreementID { get; set; }
        public int ClientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserID { get; set; }
        public bool Agreed { get; set; }
        public AccountFeatureEnum FeatureOptionID { get; set; }
    }
}
