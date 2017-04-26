using System.Web.Http;
using Conference.BL.Authentication;

namespace Conference.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Filters.Add(new BearerAuthenticationFilter());
        }
    }
}