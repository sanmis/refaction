using System.Web.Http;
using WebActivatorEx;
using refactor_me;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace refactor_me
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "refactor_me"); // version number can be provided in app settings file
                    })
                .EnableSwaggerUi(c =>
                    {
                    });
        }
    }
}
