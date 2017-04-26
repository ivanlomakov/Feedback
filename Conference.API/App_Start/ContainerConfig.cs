using System.Web.Http;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;

namespace Conference.API
{
    public static class ContainerConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

            container.RegisterWebApiControllers(configuration);
            container.RegisterPackages();

            container.Verify();

            configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}