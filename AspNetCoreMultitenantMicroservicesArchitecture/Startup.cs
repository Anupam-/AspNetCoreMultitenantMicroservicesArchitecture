using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using AspNetCoreMultitenantMicroservicesArchitecture.DependencyRegistrar;
using Autofac;
using Autofac.Multitenant;
using Microsoft.AspNetCore.Http;
using AspNetCoreMultitenantMicroservicesArchitecture.Proxies;
using AspNetCoreMultitenantMicroservicesArchitecture.UI.Proxies;
using Autofac.Extensions.DependencyInjection;

namespace AspNetCoreMultitenantMicroservicesArchitecture
{
    // http://autofac.readthedocs.io/en/latest/integration/aspnetcore.html#multitenant-support
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static MultitenantContainer ApplicationContainer { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // First, create your application-level defaults using a standard
            // ContainerBuilder, just as you are used to.
            var builder = new ContainerBuilder();
            builder.RegisterType<Controllers.HomeController>();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.RegisterType<DefaultServiceProxy>().As<IServiceProxy>().SingleInstance();

            builder.Populate(services);

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
            mtc.ConfigureTenant("1", b => b.RegisterType<Tenant1ServiceProxy>().As<IServiceProxy>().InstancePerDependency());
            mtc.ConfigureTenant("2", b => b.RegisterType<Tenant2ServiceProxy>().As<IServiceProxy>().InstancePerDependency());

            // Now you can use the multitenant container to resolve instances.
            ApplicationContainer = mtc;
            return new AutofacServiceProvider(appContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{tenant=1}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
