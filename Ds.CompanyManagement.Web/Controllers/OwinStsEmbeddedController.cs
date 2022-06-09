using System.Web.Mvc;
using Thinktecture.IdentityModel.EmbeddedSts;
using Thinktecture.IdentityModel.EmbeddedSts.WsFed;

namespace Dominion.CompanyManagement.Web.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("_sts")]
    [Route("{action=index}")] //default action 
    public class OwinStsEmbeddedController : EmbeddedStsController
    {
        public OwinStsEmbeddedController()
        {
            //EmbeddedStsConstants.Log("OwinStsEmbeddedController-CTOR.txt", "");
        } 
    }
}