using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using refactor_me.Data;
using refactor_me.Data.Interface;
using refactor_me.Logic;
using refactor_me.Logic.Interface;

namespace refactor_me
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<PersistanceFactory>().As<IPersistanceFactory>(); 
            builder.RegisterType<ProductLibrary>().As<IProductLibrary>().InstancePerLifetimeScope();
            builder.RegisterType<ProductOptionLibrary>().As<IProductOptionLibrary>().InstancePerLifetimeScope();

            ConfigureAutomapper(builder);
            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private void ConfigureAutomapper(ContainerBuilder builder)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
                         cfg.AddProfiles(new[] { "refactor-me.Logic", "refactor-me.Data" }));
            IMapper mapper = new Mapper(mapperConfig);
            builder.RegisterInstance(mapper).As<IMapper>().SingleInstance();
        }
    }
}
