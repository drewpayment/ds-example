using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.LaborManagement.Service.Api;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    public interface IApplicantTrackingAuthProvider
    {
        IOpResult<T> AuthorizeByClientIdFn<T>(Func<IOpResult<T>> functionClass, int clientId);
    }
}
