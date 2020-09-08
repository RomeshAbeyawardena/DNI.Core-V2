using DNI.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;
using DNI.Core.Services.Extensions;
using System;
using System.Text;
using Microsoft.EntityFrameworkCore;
using DNI.Core.Contracts.Builders;
using System.Reflection;
using DNI.Core.Shared.Enumerations;
using System.Security.Authentication;
using System.Runtime.Intrinsics.X86;
using System.Reactive.Subjects;

namespace TestWebApp
{
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            Action<IDefinition<Assembly>> assemblyDefinitions = assemblyDefinitions =>
                    assemblyDefinitions.DescribeAssembly<Startup>();

            services
                .AddSingleton(typeof(ISubject<>), typeof(Subject<>))
                .RegisterRepositories<SiteDbContext>((serviceProvider, dbContextOptions) => {
                    var applicationSettings = serviceProvider.GetRequiredService<ApplicationSettings>();
                    dbContextOptions.UseSqlServer(applicationSettings.DefaultConnectionString);},
                    options => { 
                        options.SingulariseTableNames = true;
                        options.EnableTracking = false; 
                        options.UseDbContextPools = true;
                        options.PoolSize = 256; })
                .RegisterServices(BuildSecurityProfiles)
                .RegisterCacheState<DateTimeOffset>()
                .RegisterAutoMapperProviders(assemblyDefinitions)
                .RegisterMediatrProviders(assemblyDefinitions);
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

    }
}
