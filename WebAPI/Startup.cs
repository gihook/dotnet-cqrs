using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebAPI.ActionCore;
using WebAPI.ActionInterfaces;
using WebAPI.Actions;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddControllers();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // TODO: investigate module registration
            /* builder.RegisterModule(new MyApplicationModule()); */
            builder.RegisterType<ActionParser>().As<IActionParser>();
            builder.RegisterType<ActionExecutor>().As<IActionExecutor>();
            // TODO: automatize
            builder.RegisterType<DummyCommand>().Keyed<IAction>(typeof(DummyCommand).Name);
            builder.RegisterType<DummyQuery>().Keyed<IAction>(typeof(DummyQuery).Name);
            builder.RegisterType<ActionInfoProvider>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
