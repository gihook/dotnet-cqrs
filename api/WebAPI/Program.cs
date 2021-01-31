using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Autofac.Extensions.DependencyInjection;
using System.IO;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var serviceProviderFactory = new AutofacServiceProviderFactory();
            var host = Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(serviceProviderFactory)
        .ConfigureWebHostDefaults(webBuilder =>
                {
                    var rootDirectory = Directory.GetCurrentDirectory();
                    webBuilder.UseContentRoot(rootDirectory)
                                .UseStartup<Startup>();
                });


            return host;
        }
    }
}
