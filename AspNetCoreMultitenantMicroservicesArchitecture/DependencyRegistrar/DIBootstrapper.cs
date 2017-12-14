using AspNetCoreMultitenantMicroservicesArchitecture.Proxies;
using AspNetCoreMultitenantMicroservicesArchitecture.UI.Proxies;
using Autofac;
using Autofac.Multitenant;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMultitenantMicroservicesArchitecture.DependencyRegistrar
{
    public class DIBootstrapper
    {
        public static void Bootstrap()
        {
            // First, create your application-level defaults using a standard
            // ContainerBuilder, just as you are used to.
            var builder = new ContainerBuilder();
            builder.RegisterType<Controllers.HomeController>();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.RegisterType<DefaultServiceProxy>().As<IServiceProxy>().SingleInstance();
            var appContainer = builder.Build();

            // Once you've built the application-level default container, you
            // need to create a tenant identification strategy. The details of this
            // are discussed later on.
            var tenantIdentifier = new RequestParameterStrategy(appContainer.Resolve<IHttpContextAccessor>());

            // Adding the tenant ID strategy into the container so controllers
            // can display output about the current tenant.
            builder.RegisterInstance(tenantIdentifier).As<ITenantIdentificationStrategy>();


            // Now create the multitenant container using the application
            // container and the tenant identification strategy.
            var mtc = new MultitenantContainer(tenantIdentifier, appContainer);

            // Configure the overrides for each tenant by passing in the tenant ID
            // and a lambda that takes a ContainerBuilder.
            mtc.ConfigureTenant('1', b => b.RegisterType<Tenant1ServiceProxy>().As<IServiceProxy>().InstancePerDependency());
            mtc.ConfigureTenant('2', b => b.RegisterType<Tenant1ServiceProxy>().As<IServiceProxy>().InstancePerDependency());

            // Now you can use the multitenant container to resolve instances.
        }
    }
}
