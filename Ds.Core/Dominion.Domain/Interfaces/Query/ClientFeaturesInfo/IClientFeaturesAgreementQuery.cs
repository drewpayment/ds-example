using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.ClientFeatures;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.ClientFeaturesInfo
{
    public interface IClientFeaturesAgreementQuery : IQuery<ClientFeaturesAgreement, IClientFeaturesAgreementQuery>
    {
        IClientFeaturesAgreementQuery ByClientId(int clientId);
        IClientFeaturesAgreementQuery ByFeatureId(AccountFeatureEnum featureId);

    }
}
