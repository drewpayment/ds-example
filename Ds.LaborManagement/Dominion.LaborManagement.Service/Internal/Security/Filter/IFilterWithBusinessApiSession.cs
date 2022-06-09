using System.Net.Http;
using System.Web.Http.Filters;
using Dominion.Core.Services.Interfaces;

namespace Dominion.LaborManagement.Service.Internal.Security.Filter
{
    public interface IFilterWithBusinessApiSession : IAuthorizationFilter
    {
        IBusinessApiSession GetBusinessApiSession(HttpRequestMessage request);
    }
}
