using DNI.Core.Services.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DNI.Core.Extensions;
using Microsoft.Extensions.Logging;
using DNI.Core.Contracts;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace TestWebApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<ApplicationSettings>()
                .RegisterServiceBroker<AppServiceBroker>()
                .AddLogging(loggerBuilder => loggerBuilder
                .AddConsole()
                .AddDatabase<SiteDbContext>(options => options
                    .ConfigureDatabaseLogManagers<DatabaseLogManager>()
                    .ConfigureLogStatusManager(serviceProvider => new LogConfiguration(serviceProvider))))
                .AddControllers();

        }

        public class LogConfiguration : ILogStatusConfiguration
        {
            public LogConfiguration(IConfiguration configuration)
            {
                configuration.Bind(this);
            }

            public IDictionary<LogLevel, bool> LogStatus { get; }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
