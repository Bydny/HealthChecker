using Hangfire;
using Hangfire.MemoryStorage;
using HealthChecker.Api.Services.BackgroundExecution;
using HealthChecker.Api.Services.BackgroundExecution.Jobs;
using HealthChecker.Api.Services.BackgroundExecution.Jobs.SignalR;
using HealthChecker.Api.Services.Interfaces;
using HealthChecker.Api.Services.Interfaces.BackgroundExecution;
using HealthChecker.Api.Services.Interfaces.BackgroundExecution.Jobs;
using HealthChecker.Api.Services.Interfaces.BackgroundExecution.Jobs.SignalR;
using HealthChecker.Api.Services.SignalR;
using HealthChecker.ServiceBus.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.SpaServices.AngularCli;

namespace HealthChecker.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private const string CorsPolicyName = "HealthChecker_Policy";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName, builder => builder
                    .WithOrigins(Configuration.GetSection("AllowedHosts").Get<string[]>())
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddSignalR();
            services.AddControllers();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddSingleton<IHealthCheckService, HealthCheckService>();

            AddBusControl(services);
            AddBackgroundJobsExecution(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(CorsPolicyName);
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllers();
                endpoints.MapHub<WestHealthCheckHub>("/health-check/west");
                endpoints.MapHub<EastHealthCheckHub>("/health-check/east");
                endpoints.MapHub<SouthHealthCheckHub>("/health-check/south");
            });

            if (Boolean.Parse(Environment.GetEnvironmentVariable("USE_SPA")))
            {
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp";

                    if (env.IsDevelopment())
                    {
                        spa.UseAngularCliServer(npmScript: "start");
                    }
                });
            }

            app.UseHangfireServer();

            UseBackgroundJobsExecution(serviceProvider);
        }

        private void AddBusControl(IServiceCollection services)
        {
            services.AddRabbitMq(cfg =>
            {
                var busConfig = Configuration.GetSection(nameof(BusConfiguration)).Get<BusConfiguration>();
                cfg
                    .AddHost(busConfig.HostName)
                    .SetUsername(busConfig.Username)
                    .SetPassword(busConfig.Password)
                    .BuildBusControl();
            });
        }

        private static void AddBackgroundJobsExecution(IServiceCollection services)
        {
            services.AddHangfire((sp, configuration) => configuration.UseMemoryStorage());
            services.AddHangfireServer();

            services.AddSingleton<IJobScheduler, JobScheduler>();
            services.AddSingleton<ISouthPingJob, SouthPingJob>();
            services.AddSingleton<IWestPingJob, WestPingJob>();
            services.AddSingleton<IEastPingJob, EastPingJob>();

            services.AddSingleton<IWestPingHubJob, WestPingHubJob>();
            services.AddSingleton<IEastPingHubJob, EastPingHubJob>();
            services.AddSingleton<ISouthPingHubJob, SouthPingHubJob>();

        }

        private static void UseBackgroundJobsExecution(IServiceProvider serviceProvider)
        {
            var jobScheduler = serviceProvider.GetService<IJobScheduler>();
            jobScheduler.ScheduleRecurringJob<ISouthPingJob>(Cron.Minutely());
            jobScheduler.ScheduleRecurringJob<IWestPingJob>(Cron.Minutely());
            jobScheduler.ScheduleRecurringJob<IEastPingJob>(Cron.Minutely());
        }

    }
}
