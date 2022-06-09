using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Xml.Linq;
using Dominion.Core.InjectionBindings.WebApi;
using Dominion.Utility.Sts.Owin;
using Dominion.WebApi.Core;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.WsFederation;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using Thinktecture.IdentityModel.EmbeddedSts;
using Thinktecture.IdentityModel.EmbeddedSts.WsFed;

namespace Dominion.CompanyManagement.Web
{
    public class Startup
    {
        public Startup()
        {
        }

        public void Configuration(IAppBuilder app)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            if(EmbeddedStsConstants.UsingEmbeddedSts)
                EmbeddedSts(app);
            else
                ServerSts(app);
        }

        private void EmbeddedSts(IAppBuilder app)
        {
            //EmbeddedStsConstants.Log("Startup-EmbeddedSts");

            //this will setup a path for getting the metedata
            app.Map("/EmbeddedStsMetadata", config =>
            {
                config.Run(context =>
                {
                    var metadataBaseUrl = context.Request.Uri.GetLeftPart(UriPartial.Scheme) + context.Request.Uri.Authority;
                    var obj = new EmbeddedTokenServiceConfiguration();            
                    var claims = EmbeddedStsUserManager.GetAllUniqueClaimTypes();
                    //var metadata = obj.GetFederationMetadata("http://localhost", claims).ToString(SaveOptions.DisableFormatting);
                    var metadata = obj.GetFederationMetadata(metadataBaseUrl, claims).ToString(SaveOptions.DisableFormatting);

                    context.Response.ContentType = "text/xml";
                    return context.Response.WriteAsync(metadata);
                });
            });

            app.SetDefaultSignInAsAuthenticationType(WsFederationAuthenticationDefaults.AuthenticationType);

            // use cookies to maintain auth credentials for all MVC & Web API requests
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = WsFederationAuthenticationDefaults.AuthenticationType
            });

            app.UseWsFederationAuthentication(
                new WsFederationAuthenticationOptions
                {
                    MetadataAddress = ConfigurationManager.AppSettings["sts:MetadataAddress"],
                    Wtrealm = ConfigurationManager.AppSettings["sts:Wtrealm"],
                    Wreply = ConfigurationManager.AppSettings["ests:Wreply"],
                    BackchannelHttpHandler = new WebRequestHandler
                    {
                        ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true
                    }
                }
            );

            AddWebApi(app);
        }

        private void ServerSts(IAppBuilder app)
        {
            //EmbeddedStsConstants.Log("Startup-ServerSts");

            //// note: order of app.Use() statements determines middleware priority
            // ----------------------------------------------------------------------
            // - AUTHENTICATION
            // ----------------------------------------------------------------------
            // add custom STS query string parameters during signout
            app.Use<DominionStsErrorCodeLogger>();

            app.SetDefaultSignInAsAuthenticationType(WsFederationAuthenticationDefaults.AuthenticationType);

            // use cookies to maintain auth credentials for all MVC & Web API requests
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = WsFederationAuthenticationDefaults.AuthenticationType
            });

            // setup Dominion STS wsfed authentication
            var options = new WsFederationAuthenticationOptions
            {
                MetadataAddress = ConfigurationManager.AppSettings["sts:MetadataAddress"],
                Wtrealm = ConfigurationManager.AppSettings["sts:Wtrealm"],
                SignOutWreply = ConfigurationManager.AppSettings["legacyRootUrl"]
                //AuthenticationMode = AuthenticationMode.Active
            };

            if (ConfigurationManager.AppSettings["owin:OverrideCertValidation"] != null
                && bool.Parse(ConfigurationManager.AppSettings["owin:OverrideCertValidation"]))
            {
                // do not validate cert if running in development environment
                options.BackchannelHttpHandler = new WebRequestHandler
                {
                    ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true
                };
            }

            app.UseWsFederationAuthentication(options);

            // checks for a 'wa=wsignoutcleanup1.0' request and initiates a signout accordingly
            app.Use<WSignoutCleanupMiddleware>();

            // checks to make sure that the user is at the correct homesite.
            app.Use<UserHomesiteValidation>();

            AddWebApi(app);
        }

        private void AddWebApi(IAppBuilder app)
        {
            var composer = new WebApiProductionComposer();

            app.UseNinjectMiddleware(composer.GetKernel)
                .UseNinjectWebApi(ConfigurationFactory.CreateApiConfig(composer));            
        }

    }
}
