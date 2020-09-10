using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using DNI.Core.Services.Managers;
using DNI.Core.Services.Implementations.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using MediatR;
using DNI.Core.Contracts.Providers;
using DNI.Core.Services.Providers;
using DNI.Core.Services.Builders;
using DNI.Core.Services.Definitions;
using DNI.Core.Contracts.Builders;
using System.Diagnostics;
using DNI.Core.Services.Implementations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DNI.Core.Services.Implementations.Cache;

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

        public static IServiceCollection RegisterCacheState<TState>(this IServiceCollection services)
        {
            services.TryAddSingleton<ICacheState<TState>, CacheState<TState>>();

            return services;
        }

        public static IServiceCollection RegisterServices(
            this IServiceCollection services, 
            Action<IServiceProvider, IEncryptionProfileDictionaryBuilder> buildSecurityProfiles = null,
            Action<MemoryDistributedCacheOptions> configureDistrubutedCacheOptions = null,
            Action<DistributedCacheEntryOptions> configureDistributedCacheEntryOptions = null,
            Action<MessagePack.MessagePackSerializerOptions> configureMessagePackSerializerOptions = null,
            IEnumerable<KeyValuePair<string, Type>> generatorKeyValuePairs = null,
            Action<IDefinition<Assembly>> scanAssembliesForGenerators = null,
            Action<Scrutor.ITypeSourceSelector> scannerConfiguration = null)
        {
            
            if(generatorKeyValuePairs != null)
            {
                services.AddSingleton(generatorKeyValuePairs);
            }
            else if (scanAssembliesForGenerators != null)
            {
                var generatorKeyValuePairsList = new List<KeyValuePair<string, Type>>();
                var assemblyDefinitions =  AssemblyDefinition.Default;
                scanAssembliesForGenerators(assemblyDefinitions);
                foreach(var serviceType in assemblyDefinitions.CollectServices<IValueGenerator>())
                {
                    services.AddSingleton(serviceType);
                    generatorKeyValuePairsList.Add(KeyValuePair.Create(serviceType.FullName, serviceType));
                }

                generatorKeyValuePairs = generatorKeyValuePairsList.ToArray();
            }
            
            var messagePackSerializerOptions = MessagePack.MessagePackSerializerOptions.Standard;

            configureMessagePackSerializerOptions?.Invoke(messagePackSerializerOptions);
            services.AddSingleton(messagePackSerializerOptions);

            var distributedCacheEntryOptions = new DistributedCacheEntryOptions();

            configureDistributedCacheEntryOptions?.Invoke(distributedCacheEntryOptions);

            services.AddSingleton(distributedCacheEntryOptions);

            if(configureDistrubutedCacheOptions != null)
            { 
                services.AddDistributedMemoryCache(configureDistrubutedCacheOptions);
            }
            else
            {
                services.AddDistributedMemoryCache();
            }

            services.RegisterServiceBroker<DefaultServiceBroker>();

            if (buildSecurityProfiles != null)
            {
                var securityProfilesDictionaryBuilder = new EncryptionProfileDictionaryBuilder();
                services
                    .AddSingleton<IEncryptionProfileManager>(serviceProvider => { 
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
            var valueGeneratorConcreteTypes = TypeCollector.Default.Collect<IValueGenerator>(describe => describe.DescribeAssembly<T>());

            foreach(var valueGeneratorConcreteType in valueGeneratorConcreteTypes)
            {
                services.AddSingleton(valueGeneratorConcreteType);
            }

            return valueGeneratorConcreteTypes
                .Select(valueGenerator => new KeyValuePair<string, Type>(valueGenerator.FullName, valueGenerator));

        }

        public static IEnumerable<KeyValuePair<string, Type>> ScanAndRegisterGenerators(Type serviceType, IServiceCollection services)
        {
            var valueGeneratorConcreteTypes = AssemblyDefinition.Default.Add(Assembly.GetAssembly(serviceType)).CollectServices<IValueGenerator>();

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

            var assemblyDefinitions = AssemblyDefinition.Default;
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

            var assemblyDefinitions = AssemblyDefinition.Default;
            obtainAssemblyDefinitions(assemblyDefinitions);
            return services.AddMediatR(
                assemblyDefinitions.Definitions.ToArray(), 
                configuremediatRServiceConfiguration);
        }
    }
}
