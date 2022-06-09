using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Dominion.Authentication.Intermediate.Util;
using Dominion.Core.Services.Interfaces;

namespace Dominion.LaborManagement.Service.Internal.Security.Filter
{
    public class IndeedAuthorizationFilter : IFilterWithBusinessApiSession
    {
        private const int IndeedApiAccountId = 1;

        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            // only apply this authorization when our custom attribute is found on the controller or endpoint
            if (!(actionContext.ActionDescriptor.GetCustomAttributes<EnsureRequestIsFromIndeedAttribute>().Any()
                  || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<EnsureRequestIsFromIndeedAttribute>().Any()))
            {
                return await continuation(); 
            }

            // make sure the header is present
            if (!actionContext.Request.Headers.TryGetValues("X-Indeed-Signature", out var values))
                return new HttpResponseMessage(HttpStatusCode.Forbidden);

            var signature = values.First();



            // Unfortunately we can't use the constructor to inject an IBusinessApi session.
            // Web api filters are singletons which means that if we were to try to use the constructor
            // to inject an IBusinessApi session we would only have a dbcontext in that session for the first request
            var context = GetBusinessApiSession(actionContext.Request);

            var secret = context.UnitOfWork.ApiRepository.GetApiAccountQuery().ByApiAccountId(IndeedApiAccountId).ExecuteQuery().FirstOrDefault()?
                .ApiKey;

            // make sure we have a secret we can use to create the hash
            if(secret.IsNullOrEmpty()) return new HttpResponseMessage(HttpStatusCode.Forbidden);


            // compute and compare hashes
            var inputBytes = await actionContext.Request.Content.ReadAsByteArrayAsync();
            var keyBytes = Encoding.UTF8.GetBytes(secret);
            var myHmacsha1 = new HMACSHA1(keyBytes);
            var hash = myHmacsha1.ComputeHash(inputBytes);
            var hashInBase64 = Convert.ToBase64String(hash);

            if (hashInBase64.CompareTo(signature) == 0)
            {
                return await continuation();
            }

            return new HttpResponseMessage(HttpStatusCode.Forbidden);
        }

        public virtual IBusinessApiSession GetBusinessApiSession(HttpRequestMessage request)
        {
            return request.GetDependencyScope().GetService(typeof(IBusinessApiSession)) as IBusinessApiSession;
        }

        public bool AllowMultiple => false;
    }
}
