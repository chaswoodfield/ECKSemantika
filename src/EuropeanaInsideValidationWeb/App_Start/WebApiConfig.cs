using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace EuropeanaInsideValidationWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.Routes.MapHttpRoute(
                name: "Profiles",
                routeTemplate: "validation/{provider}/profiles/{name}",
                defaults: new { controller="Profiles", name = RouteParameter.Optional}
            );

            config.Routes.MapHttpRoute(
                name: "Validation",
                routeTemplate: "validation/{provider}/single/validate/{name}",
                defaults: new {controller = "Validation"}
                );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
