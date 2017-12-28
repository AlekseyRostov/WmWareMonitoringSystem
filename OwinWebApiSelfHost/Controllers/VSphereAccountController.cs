using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using OwinWebApiSelfHost.Model;
using WMWareApi;

namespace OwinWebApiSelfHost.Controllers
{
    [Authorize]
    [RoutePrefix("api/vSphereAccount/")]
    public class VSphereAccountController : ApiController
    {
        public static string SessionId = "vSphereSessionId";
        public static string ServiceUrl = "vSphereServiceUrl";

        [Route("Register")]
        public async Task<HttpResponseMessage> Register(VmAccountBindingModels model)
        {
            if (!ModelState.IsValid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            VmWareClient vmWareClient = new VmWareClient(model.Url, model.Login, model.Password);
            string sessionId = vmWareClient.Connect();

            var resp = new HttpResponseMessage(HttpStatusCode.OK);

            var cookie = new CookieHeaderValue(SessionId, sessionId);
            cookie.Expires = DateTimeOffset.Now.AddDays(1);
            cookie.Domain = Request.RequestUri.Host;
            cookie.Path = "/";

            resp.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            return resp;
        }
    }
}
