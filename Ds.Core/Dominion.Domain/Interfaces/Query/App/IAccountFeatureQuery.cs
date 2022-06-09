using Dominion.Domain.Entities.Misc;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query.App
{
    public interface IAccountFeatureQuery : IQuery<AccountFeatureInfo, IAccountFeatureQuery>
    {
    }
}
