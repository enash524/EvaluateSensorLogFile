using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using EvaluateSensorLog.Application;
using EvaluateSensorLog.ClassLibrary;
using EvaluateSensorLog.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EvaluateSensorLog.ConsoleApp
{
    /// <summary>
    /// Starts the console application
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Use this method to add services to the container
        /// </summary>
        /// <returns>Services container</returns>
        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            IConfiguration config = LoadConfiguration();

            // Add the config to our DI container for later user
            services
                .AddSingleton(config)
                .AddLogging(configure =>
                {
                    configure
                        .AddConfiguration(config.GetSection("Logging"))
                        .AddConsole(config =>
                        {
                            config.TimestampFormat = "yyyy-MM-dd hh:mm:ss tt ";
                        });
                })
                .AddApplication()
                .AddClassLibrary()
                .AddData()
                .AddScoped<IFileSystem, FileSystem>()
                .AddTransient<ConsoleApplication>();

            return services;
        }

        /// <summary>
        /// Use this method to load the configuration file
        /// </summary>
        /// <returns>Set of key/value application configuration properties</returns>
        private static IConfiguration LoadConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

        /// <summary>
        /// Main entry point to ConsoleApp
        /// </summary>
        /// <param name="args">Collection of command line arguments. Currently unused.</param>
        private static async Task Main(string[] args)
        {
            // Create service collection and configure our services
            IServiceCollection services = ConfigureServices();

            // Generate a provider
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<ILogger<Program>>().LogInformation("serviceProvider built");

            // Kick off our actual code
            await serviceProvider.GetService<ConsoleApplication>().Run("input.txt");
        }
    }
}