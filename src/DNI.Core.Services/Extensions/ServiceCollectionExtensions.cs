using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using DNI.Core.Services.Managers;
using DNI.Core.Services.Implementations.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;
using System.Reflection;
using AutoMapper;
using MediatR;
using DNI.Core.Shared.Extensions;
using DNI.Core.Contracts.Providers;
using DNI.Core.Services.Providers;
using DNI.Core.Shared.Enumerations;
using DNI.Core.Services.Builders;
using DNI.Core.Domains;
using DNI.Core.Services.Attributes;
using DNI.Core.Services.Implementations.Generators;
using DNI.Core.Services.Definitions;
using DNI.Core.Contracts.Builders;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using DNI.Core.Services.Hosts;
using DNI.Core.Shared.Attributes;
using System.Diagnostics;

namespace DNI.Core.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterRepositories<TDbContext>(
            this IServiceCollection services, 
            IRepositoryOptions repositoryOptions,
            Action<IServiceProvider, DbContextOptionsBuilder> dbContextOptionsBuilder,
            params Type[] entityTypes)
            where TDbContext : DbContext
        {
            services.AddSingleton(repositoryOptions);

            if (repositoryOptions.UseDbContextPools)
            {
                services.AddDbContextPool<TDbContext>(dbContextOptionsBuilder, repositoryOptions.PoolSize);
            }
            else
            { 
                services.AddDbContext<TDbContext>(dbContextOptionsBuilder);
            }

            var repositoryType = typeof(IRepository<>);
            var asyncRepositoryType = typeof(IAsyncRepository<>);
            var concreteRepositoryType = typeof(EntityFrameworkRepository<,>);
            var concreteAsyncRepositoryType = typeof(AsyncEntityFrameworkRepository<,>);

            foreach(var entityType in entityTypes)
            {
                var entityRepositoryType = repositoryType.MakeGenericType(entityType);
                var concreteEntityRepositoryType = concreteRepositoryType.MakeGenericType(typeof(TDbContext), entityType);

                var asyncEntityRepositoryType = asyncRepositoryType.MakeGenericType(entityType);
                var concreteAsyncEntityRepositoryType = concreteAsyncRepositoryType.MakeGenericType(typeof(TDbContext), entityType);

                services.AddScoped(entityRepositoryType, concreteEntityRepositoryType);
                services.AddScoped(asyncEntityRepositoryType, concreteAsyncEntityRepositoryType);
            }

            return services;
        }

        public static IServiceCollection RegisterRepositories<TDbContext>(
            this IServiceCollection services, 
            Action<IServiceProvider, DbContextOptionsBuilder> dbContextOptionsBuilder,
            Action<IRepositoryOptions> repositoryOptions = null)
            where TDbContext : DbContext
        {
            var genericTypeDbSet = typeof(DbSet<>);
            var dbContextType = typeof(TDbContext);
            var properties = dbContextType.GetProperties();
            var dbSetProperties = properties.Where(property => property.PropertyType.IsGenericType);

            var entities = dbSetProperties.Select(property => property.PropertyType.GetGenericArguments().FirstOrDefault());
            
            var respositoryOptions = RepositoryOptionsBuilder.Build(repositoryOptions ?? RepositoryOptionsBuilder.Default);
            var entitiesArray = entities.ToArray();

            return RegisterRepositories<TDbContext>(
                services, 
                respositoryOptions,
                dbContextOptionsBuilder,
                entitiesArray);
        }

        public static IServiceCollection RegisterServices(
            this IServiceCollection services, 
            Action<IServiceProvider, IEncryptionProfileDictionaryBuilder> buildSecurityProfiles = null,
            IEnumerable<KeyValuePair<string, Type>> generatorKeyValuePairs = null,
            Action<Scrutor.ITypeSourceSelector> scannerConfiguration = null)
        {
            var internalGeneratorKeyValuePairs = ScanAndRegisterGenerators<RepositoryOptions>(services);
            if(generatorKeyValuePairs == null)
            {
                generatorKeyValuePairs = internalGeneratorKeyValuePairs;
            }
            else
            {
                generatorKeyValuePairs = generatorKeyValuePairs.Append(internalGeneratorKeyValuePairs);
            }

            bool HasAttribute(Type type)
            {
                var ignoreScanningAttributeType = typeof(IgnoreScanningAttribute);
                var attributes = type.CustomAttributes;
                Debug.WriteLine("Inspecting {0}...", args: type.Name);
                foreach(var attribute in attributes)
                {
                    Debug.WriteLine("{0}: {1}", type.Name, attribute.AttributeType.Name);
                }
                
                var result = attributes.Count() == 0 
                    || attributes.Any(attribute => attribute.AttributeType != ignoreScanningAttributeType);
                return result;
            }
            
            services
                .AddSingleton<ISecurityTokenValidator>(new JwtSecurityTokenHandler())
                .AddSingleton<ISystemClock, SystemClock>()
                .AddSingleton<IValueGeneratorManager>(serviceProvider => new ValueGeneratorManager(generatorKeyValuePairs))
                .Scan(scan => scan.FromAssemblyOf<RepositoryOptions>()
                .AddClasses(filter => filter.Where(HasAttribute), true)
                .AsMatchingInterface());

            if (buildSecurityProfiles != null)
            {
                var securityProfilesDictionaryBuilder = new EncryptionProfileDictionaryBuilder();

                services.AddSingleton<IEncryptionProfileManager>(serviceProvider => { 
                    buildSecurityProfiles(serviceProvider, securityProfilesDictionaryBuilder); 
                    return new EncryptionProfileManager(securityProfilesDictionaryBuilder); 
                });
            }

            if(scannerConfiguration != null)
            {
                services.Scan(scannerConfiguration);
            }

            foreach(var service in services)
            {
                if(service.ImplementationType != null)
                { 
                    Debug.WriteLine("{0}: {1}", service.ServiceType.Name, service.ImplementationType.Name);
                }
            }

            return services;
        }

        public static IServiceCollection RegisterServiceBroker<TServiceBroker>(this IServiceCollection services, params object[] serviceBrokerConstructorArguments)
            where TServiceBroker : IServiceBroker
        {
            var serviceBroker = Activator.CreateInstance(typeof(TServiceBroker), serviceBrokerConstructorArguments) as IServiceBroker;

            serviceBroker.RegisterServices(services, serviceBroker.GetServiceRegistrations());

            return services;
        }

        public static IEnumerable<KeyValuePair<string, Type>> ScanAndRegisterGenerators<T>(IServiceCollection services)
        {
            var valueGeneratorConcreteTypes = typeof(T)
                .Assembly.GetTypes()
                .Where(type => type.GetInterfaces().Any(interfaceType => interfaceType == typeof(IValueGenerator)));

            foreach(var valueGeneratorConcreteType in valueGeneratorConcreteTypes)
            {
                services.AddSingleton(valueGeneratorConcreteType);
            }

            return valueGeneratorConcreteTypes
                .Select(valueGenerator => new KeyValuePair<string, Type>(valueGenerator.FullName, valueGenerator));

        }

        public static IServiceCollection RegisterAutoMapperProviders(
            this IServiceCollection services, 
            Action<IDefinition<Assembly>> obtainAssemblyDefinitions,
            Action<IServiceProvider, IMapperConfigurationExpression> configureAutomapper = null)
        {
            services.AddSingleton<IMapperProvider, AutoMapperProvider>();

            var assemblyDefinitions = new AssemblyDefinition();
            obtainAssemblyDefinitions(assemblyDefinitions);

            if(configureAutomapper != null)
            { 
                services.AddAutoMapper(configureAutomapper, assemblyDefinitions.Definitions);
            }
            else
            {
                services.AddAutoMapper(assemblyDefinitions.Definitions);
            }
            
            return services;
        }

        public static IServiceCollection RegisterMediatrProviders(
            this IServiceCollection services,
            Action<IDefinition<Assembly>> obtainAssemblyDefinitions,
            Action<MediatRServiceConfiguration> configuremediatRServiceConfiguration = null)
        {
            services.AddSingleton<IMediatorProvider, MediatrProvider>();

            var assemblyDefinitions = new AssemblyDefinition();
            obtainAssemblyDefinitions(assemblyDefinitions);
            return services.AddMediatR(
                assemblyDefinitions.Definitions.ToArray(), 
                configuremediatRServiceConfiguration);
        }
    }
}
