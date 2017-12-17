using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using AspNetCoreMultitenantMicroservicesArchitecture.DependencyRegistrar;
using Autofac;
using Autofac.Multitenant;
using Microsoft.AspNetCore.Http;
using Autofac.Extensions.DependencyInjection;
using System.Reflection;
using Proxies;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Controllers;
using AspNetCoreMultitenantMicroservicesArchitecture.UI;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Internal;

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
            //services.AddScoped<IControllerActivator, MyControllerActivator>();
            services.AddScoped<IActionSelector, TenantActionSelector>();

            // First, create your application-level defaults using a standard
            // ContainerBuilder, just as you are used to.
            var builder = new ContainerBuilder();
            // default controller dependency
            // however this is not required as framework will register all controller present or referenced in application
            //builder.RegisterType<DefaultControllers.HomeController>();
            // default service dependency
            builder.RegisterType<DefaultServiceProxy>().As<IServiceProxy>();

            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.RegisterType<TenantResolverStrategy>().As<ITenantIdentificationStrategy>();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).InstancePerLifetimeScope();

            builder.Populate(services);

            var appContainer = builder.Build();


            // Now create the multitenant container 
            var mtc = new MultitenantContainer(appContainer.Resolve<ITenantIdentificationStrategy>(), appContainer);

            // Configure the overrides for each tenant by passing in the tenant ID
            mtc.ConfigureTenant("tenant1", b =>
            {
                b.RegisterType<Tenant1ServiceProxy>().As<IServiceProxy>().InstancePerDependency();
            });
            mtc.ConfigureTenant("tenant2", b =>
            {
                b.RegisterType<Tenant2ServiceProxy>().As<IServiceProxy>().InstancePerDependency();
            });
            

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
                    template: "{tenant=default}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
