using HealthChecker.Contracts.Interfaces.Requests;
using HealthChecker.Contracts.Interfaces.Responses;
using HealthChecker.ServiceBus.Extensions;
using HealthChecker.ServiceBus.Interfaces.BusControl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace HealthChecker.East
{
    class Program
    {
        public static IConfigurationRoot Configuration;

        static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                Configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetParent(Environment.CurrentDirectory).Parent?.Parent?.FullName)
                    .AddJsonFile("appsettings.json", false)
                    .Build();

                Log.Information("Creating service collection");
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);

                Log.Information("Building service provider");
                var serviceProvider = serviceCollection.BuildServiceProvider();

                Log.Information("Starting service");

                using (var busControl = serviceProvider.GetService<IBusControl>())
                {
                    CreateHostBuilder(args).Build().Run();
                }

                Log.Information("Ending service");
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error running service");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Add logging
            serviceCollection.AddSingleton(LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(dispose: true);
            }));

            serviceCollection.AddLogging();
            serviceCollection.AddRabbitMq(cfg =>
            {
                var busConfig = Configuration.GetSection(nameof(BusConfiguration)).Get<BusConfiguration>();
                cfg
                    .AddHost(busConfig.HostName)
                    .SetUsername(busConfig.Username)
                    .SetPassword(busConfig.Password)
                    .BuildBusControl();
                cfg.AddConsumer<EasternConsumer, IEastRequest, IEastResponse>();
            });
        }



        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args);
    }
}
