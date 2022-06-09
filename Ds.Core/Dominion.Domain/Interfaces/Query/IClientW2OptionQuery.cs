using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;
using System;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientW2OptionQuery : IQuery<ClientW2Options, IClientW2OptionQuery>
    {
        IClientW2OptionQuery ByClientId(int clientId);
        IClientW2OptionQuery OrderByTaxYearDescending();
        IClientW2OptionQuery ByTaxYear(DateTime taxYear);
    }
}
