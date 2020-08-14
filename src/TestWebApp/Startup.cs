using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DNI.Core.Contracts;
using DNI.Core.Services.Builders;
using DNI.Core.Services.Extensions;
using DNI.Core.Shared.Enumerations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestWebApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Action<IAssemblyDefinition> assemblyDefinitions = assemblyDefinitions =>
                    assemblyDefinitions.GetAssembly<Startup>();

            services
                .RegisterRepositories<SiteDbContext>(dbContextOptions =>
                    dbContextOptions.UseSqlServer(""))
                .RegisterServices(BuildSecurityProfiles)
                .RegisterAutoMapperProviders(assemblyDefinitions)
                .RegisterMediatrProviders(assemblyDefinitions);

        }

        private void BuildSecurityProfiles(IDictionaryBuilder<EncryptionClassification, IEncryptionProfile> builder,
            IServiceProvider serviceProvider)
        {
            var applicationSettings = serviceProvider.GetRequiredService<ApplicationSettings>();
            builder.Add(EncryptionClassification.Personal, EncryptionProfileBuilder
                .BuildProfile(profile => { 
                    profile.Encoding = Encoding.ASCII;
                    profile.InitialVector = Convert.FromBase64String(applicationSettings.InitialVector);
                    profile.Key = Convert.FromBase64String(applicationSettings.InitialVector);
                    profile.SymmetricAlgorithmName = nameof(Aes);
                }));
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
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
