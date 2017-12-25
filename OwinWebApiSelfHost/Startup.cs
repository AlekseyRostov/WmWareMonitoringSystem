﻿using System.Web.Http;
using Owin;

namespace OwinWebApiSelfHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var webApiConfiguration = ConfigureWebApi();

            // Use the extension method provided by the WebApi.Owin library:
            app.UseWebApi(webApiConfiguration);
        }

        private HttpConfiguration ConfigureWebApi()
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional});
            return config;
        }
    }
}