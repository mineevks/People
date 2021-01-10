using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Utilities;

namespace People.Api
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            //var path = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()!.Location);

            try
            {
                var host = CreateHostBuilder(args).Build();
                await host.RunAsync();
            }
            catch (Exception exception)
            {
                LoggerStatic.Logger.Fatal($"Exception: {exception}");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((HostBuilderContext hostBuilderContext, IConfigurationBuilder config) =>
                {
                    var environment = hostBuilderContext.HostingEnvironment;
                    //var pathOfCommonSettingsFile = StringConverter.GetContentRootPath(environment.ContentRootPath);
                    var pathOfCommonSettingsFile = environment.ContentRootPath;
                    var fullPathOfCommonSettingsFile = Path.Combine(pathOfCommonSettingsFile, "appsettings.json");


                    config.AddJsonFile(fullPathOfCommonSettingsFile, optional: false);
                    config.AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    
                    webBuilder.UseUrls("http://0.0.0.0:5000");
                });


        /*
        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }
                    
                    );
        */

    }


    /*
    public static void Main(string[] args)
    {
        BuildWebHost(args)
            .Run();
    }


    public static IWebHost BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((webHostBuilderContext, configurationbuilder) =>
            {
                var environment = webHostBuilderContext.HostingEnvironment;

                var pathOfCommonSettingsFile = StringConverter.GetContentRootPath(environment.ContentRootPath);

                var fullPathOfCommonSettingsFile = Path.Combine(pathOfCommonSettingsFile, "appsettings.json");

                configurationbuilder.AddJsonFile(fullPathOfCommonSettingsFile, optional: false);
                configurationbuilder.AddEnvironmentVariables();
            })
            .UseStartup<Startup>()
            .UseKestrel(options =>
            {
                options.Limits.MinRequestBodyDataRate = new MinDataRate(100, TimeSpan.FromSeconds(200));
            })


            .CaptureStartupErrors(true)
            .UseUrls("http://0.0.0.0:5060")
            .Build();
}

    */


	}
