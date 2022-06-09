using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IExternalApplicationIdentityQuery : IQuery<ExternalApplicationIdentity, IExternalApplicationIdentityQuery>
    {
        IExternalApplicationIdentityQuery ByExternalApplicationId(string id);
    }
}
