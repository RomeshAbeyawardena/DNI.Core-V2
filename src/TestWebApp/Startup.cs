using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DNI.Core.Contracts;
using DNI.Core.Contracts.Builders;
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
            Action<IDefinition<Assembly>> assemblyDefinitions = assemblyDefinitions =>
                    assemblyDefinitions.DescribeAssembly<Startup>();

            services
                .AddSingleton<ApplicationSettings>()
                .RegisterRepositories<SiteDbContext>((serviceProvider, dbContextOptions) => {
                    var applicationSettings = serviceProvider.GetRequiredService<ApplicationSettings>();
                    dbContextOptions.UseSqlServer(applicationSettings.DefaultConnectionString);},
                    options => { 
                        options.SingulariseTableNames = true;
                        options.EnableTracking = false; 
                        options.UseDbContextPools = true;
                        options.PoolSize = 256; })
                .RegisterServices(BuildSecurityProfiles)
                .RegisterAutoMapperProviders(assemblyDefinitions)
                .RegisterMediatrProviders(assemblyDefinitions)
                .AddControllers();

        }

        private void BuildSecurityProfiles(IServiceProvider serviceProvider, IEncryptionProfileDictionaryBuilder builder)
        {
            var applicationSettings = serviceProvider.GetRequiredService<ApplicationSettings>();
            builder.Add(EncryptionClassification.Personal, profile =>
            {
                profile.Encoding = Encoding.ASCII;
                profile.InitialVector = Convert.FromBase64String(applicationSettings.InitialVector);
                profile.Key = Convert.FromBase64String(applicationSettings.PersonalKey);
                profile.Salt = Convert.FromBase64String(applicationSettings.Salt);
                profile.HashAlgorithmType = HashAlgorithmType.Sha512;
                profile.SymmetricAlgorithmName = nameof(Aes);

                return profile;
            }).Add(EncryptionClassification.Common, profile => {
                profile.Encoding = Encoding.ASCII;
                profile.InitialVector = Convert.FromBase64String(applicationSettings.InitialVector);
                profile.Key = Convert.FromBase64String(applicationSettings.CommonKey);
                profile.Salt = Convert.FromBase64String(applicationSettings.Salt);
                profile.HashAlgorithmType = HashAlgorithmType.Sha512;
                profile.SymmetricAlgorithmName = nameof(Aes);

                return profile;
            }).Add(EncryptionClassification.Shared, profile => {
                profile.Encoding = Encoding.ASCII;
                profile.InitialVector = Convert.FromBase64String(applicationSettings.InitialVector);
                profile.Key = Convert.FromBase64String(applicationSettings.SharedKey);
                profile.Salt = Convert.FromBase64String(applicationSettings.Salt);
                profile.HashAlgorithmType = HashAlgorithmType.Sha512;
                profile.SymmetricAlgorithmName = nameof(Aes);

                return profile;
            });
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
