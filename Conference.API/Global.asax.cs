using System;
using System.Web;
using System.Web.Http;
using FluentValidation.WebApi;

namespace Conference.API
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            FluentValidationModelValidatorProvider.Configure(GlobalConfiguration.Configuration);

            ContainerConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}