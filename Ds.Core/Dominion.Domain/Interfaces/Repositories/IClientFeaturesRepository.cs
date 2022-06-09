using System;
using Dominion.Domain.Interfaces.Query.ClientFeaturesInfo;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface IClientFeaturesRepository : IRepository, IDisposable
    {
        IClientFeaturesAgreementQuery QueryClientFeaturesAgreement();
    }
}
