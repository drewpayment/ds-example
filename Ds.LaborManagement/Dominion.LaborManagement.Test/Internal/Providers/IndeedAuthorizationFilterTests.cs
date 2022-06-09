using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.Api;
using Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply;
using Dominion.LaborManagement.Service.Internal.Security.Filter;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using HttpActionDescriptor = System.Web.Http.Controllers.HttpActionDescriptor;

namespace Dominion.LaborManagement.Test.Internal.Providers
{
    public class IndeedAuthorizationFilterTests
    {
        private IPrincipal _principal;
        private string _secret;
        private Mock<IBusinessApiSession> _session;
        private HttpActionContext ctx;
        private Mock<IndeedAuthorizationFilter> target;
        private Mock<HttpActionDescriptor> actionDescriptorMock;
        private Mock<HttpControllerDescriptor> controllerDescriptorMock;
        private readonly Func<Task<HttpResponseMessage>> Continuation = () => Task.Factory.StartNew(() => new HttpResponseMessage() { StatusCode = HttpStatusCode.OK });

        [SetUp]
        public void SetUp()
        {
            _principal = new GenericPrincipal(new GenericIdentity("TestName"), new string[0]);
            _secret = "abc123";
            _session = new Mock<IBusinessApiSession>();
            _session.Setup(x => x.UnitOfWork.ApiRepository.GetApiAccountQuery().ByApiId(2).ExecuteQuery())
                .Returns(() => new ApiAccount[] {new ApiAccount()});
            target = new Mock<IndeedAuthorizationFilter> {CallBase = true};  // we need to test this class but the 'GetBusinessApiSession' method needs to be mocked
            target.Setup(x => x.GetBusinessApiSession(It.IsAny<HttpRequestMessage>())).Returns(() => _session.Object);

            var request = new HttpRequestMessage(HttpMethod.Post, "");

            var application = new IndeedApplication();
            var json = JsonConvert.SerializeObject(application);
            var content = new StringContent(json);

            request.Content = content;

            var keyBytes = Encoding.UTF8.GetBytes(_secret);
            var inputBytes = Encoding.UTF8.GetBytes(request.Content.ReadAsStringAsync().Result);

            var myHmacsha1 = new HMACSHA1(keyBytes);
            var hash = myHmacsha1.ComputeHash(inputBytes);
            var hashInBase64 = Convert.ToBase64String(hash);


            request.Headers.Add("X-Indeed-Signature", hashInBase64);

            var controllerContext = new Mock<HttpControllerContext>();

            controllerContext.Object.Request = request;

            actionDescriptorMock = new Mock<HttpActionDescriptor>();
            controllerDescriptorMock = new Mock<HttpControllerDescriptor>();


            actionDescriptorMock.Setup(x => x.GetCustomAttributes<EnsureRequestIsFromIndeedAttribute>()).Returns(() =>
                new Collection<EnsureRequestIsFromIndeedAttribute>(){new EnsureRequestIsFromIndeedAttribute()});
            controllerDescriptorMock.Setup(x => x.GetCustomAttributes<EnsureRequestIsFromIndeedAttribute>())
                .Returns(() => new Collection<EnsureRequestIsFromIndeedAttribute>());
            controllerContext.Object.ControllerDescriptor = controllerDescriptorMock.Object;

            ctx = new HttpActionContext { ControllerContext = controllerContext.Object, ActionDescriptor = actionDescriptorMock.Object };
        }

        [Test]
        public void Test_Should_Allow_Request_When_Signature_Valid()
        {
            _session.Setup(x =>
                x.UnitOfWork.ApiRepository.GetApiAccountQuery().ByApiAccountId(It.IsAny<int>()).ExecuteQuery()).Returns(
                () => new []
                {
                    new ApiAccount()
                    {
                        ApiKey = _secret
                    }
                });

            Task<HttpResponseMessage> Continuation() => Task.Factory.StartNew(() => new HttpResponseMessage() {StatusCode = HttpStatusCode.OK});

            var result = target.Object.ExecuteAuthorizationFilterAsync(ctx, CancellationToken.None, Continuation).Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public void Test_Should_Forbid_Request_When_Computed_Signature_Is_Different()
        {
            _session.Setup(x =>
                x.UnitOfWork.ApiRepository.GetApiAccountQuery().ByApiAccountId(It.IsAny<int>()).ExecuteQuery()).Returns(
                () => new[]
                {
                    new ApiAccount()
                    {
                        ApiKey = _secret + "someOtherPartThatDoesntMatchOurSecret"
                    }
                });

            var result = target.Object.ExecuteAuthorizationFilterAsync(ctx, CancellationToken.None, Continuation).Result;

            Assert.AreEqual(HttpStatusCode.Forbidden, result.StatusCode);
        }

        [Test]
        public void Test_Should_Forbid_Request_When_Header_Missing()
        {
            _session.Setup(x =>
                x.UnitOfWork.ApiRepository.GetApiAccountQuery().ByApiAccountId(It.IsAny<int>()).ExecuteQuery()).Returns(
                () => new[]
                {
                    new ApiAccount()
                    {
                        ApiKey = _secret
                    }
                });

            ctx.ControllerContext.Request.Headers.Clear();

            var result = target.Object.ExecuteAuthorizationFilterAsync(ctx, CancellationToken.None, Continuation).Result;

            Assert.AreEqual(HttpStatusCode.Forbidden, result.StatusCode);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Test_Should_Forbid_Request_When_Header_Present_But_Value_Missing(string value)
        {
            _session.Setup(x =>
                x.UnitOfWork.ApiRepository.GetApiAccountQuery().ByApiAccountId(It.IsAny<int>()).ExecuteQuery()).Returns(
                () => new[]
                {
                    new ApiAccount()
                    {
                        ApiKey = _secret
                    }
                });

            ctx.ControllerContext.Request.Headers.Clear();
            ctx.ControllerContext.Request.Headers.Add("X-Indeed-Signature", value);

            var result = target.Object.ExecuteAuthorizationFilterAsync(ctx, CancellationToken.None, Continuation).Result;

            Assert.AreEqual(HttpStatusCode.Forbidden, result.StatusCode);
        }

        [Test]
        public void Test_Forbid_Request_When_ApiAccountMissing_FromDb()
        {
            _session.Setup(x =>
                x.UnitOfWork.ApiRepository.GetApiAccountQuery().ByApiAccountId(It.IsAny<int>()).ExecuteQuery()).Returns(
                () => new ApiAccount[0]);

            var result = target.Object.ExecuteAuthorizationFilterAsync(ctx, CancellationToken.None, Continuation).Result;
            Assert.AreEqual(HttpStatusCode.Forbidden, result.StatusCode);
        }

        /// <summary>
        /// Testing the situation where we are trying to make a request to an endpoint in another controller that has nothing to do with our Indeed stuff.
        /// In this case the endpoint will probably not have the <see cref="EnsureRequestIsFromIndeedAttribute"/> added to it and there will most likely be no 'x-indeed-signature'
        /// </summary>
        [Test]
        public void Test_Should_Allow_Request_If_IndeedAuthorizeAttribute_Not_Present()
        {
            _session.Setup(x =>
                x.UnitOfWork.ApiRepository.GetApiAccountQuery().ByApiAccountId(It.IsAny<int>()).ExecuteQuery()).Returns(
                () => new [] {
                    new ApiAccount()
                    {
                        ApiKey = _secret
                    }
                });


            var actionDescriptorMockNoAttribute = new Mock<HttpActionDescriptor>();

            actionDescriptorMockNoAttribute.Setup(x => x.GetCustomAttributes<EnsureRequestIsFromIndeedAttribute>()).Returns(() =>
                new Collection<EnsureRequestIsFromIndeedAttribute>());
            ctx.ActionDescriptor = actionDescriptorMockNoAttribute.Object;

            ctx.ControllerContext.Request.Headers.Clear();
            

            var result = target.Object.ExecuteAuthorizationFilterAsync(ctx, CancellationToken.None, Continuation).Result;
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
